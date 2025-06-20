using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CVisionary.Data;
using CVisionary.DTOs;
using CVisionary.Models;
using CVisionary.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVisionary.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioParser _parser;

        public PortfolioController(
            ApplicationDbContext context,
            IPortfolioRepository portfolioRepository,
            IPortfolioParser parser)
        {
            _context = context;
            _portfolioRepository = portfolioRepository;
            _parser = parser;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        public IActionResult Home()
        {
            return View();  
        }

        // GET: /Portfolio
        public IActionResult Index()
        {
            var userId = GetUserId();
            ViewBag.Portfolios = _portfolioRepository.GetAllPortfolios(userId);
            return View();
        }

        // GET: /Portfolio/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = _context.Services.ToList();
            return View();
        }

        // POST: /Portfolio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PortfolioCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = _context.Services.ToList();
                return View(dto);
            }

            try
            {
                // 1) AI-parse the freeform text to extract personal fields & enhanced summary
                var parsed = await _parser.ParsePortfolioPersonalInfoAsync(dto.PersonalInfoText);
                // `parsed` should be a custom class or tuple with FirstName, LastName, Email, etc., and EnhancedSummary

                var portfolio = new Portfolio
                {
                    // Personal Info extracted by AI
                    FirstName = parsed.FirstName,
                    SecondName = parsed.SecondName,
                    ThirdName = parsed.ThirdName,
                    LastName = parsed.LastName,
                    Email = parsed.Email,
                    PhoneNumber = parsed.PhoneNumber,
                    LinkedInLink = parsed.LinkedInLink,
                    GithubLink = parsed.GithubLink,
                    FacebookLink = parsed.FacebookLink,
                    InstagramLink = parsed.InstagramLink,
                    Address = parsed.Address,
                    DateOfBirth = parsed.DateOfBirth,

                    // Enhanced summary from parser
                    Summary = parsed.EnhancedSummary,

                    // Ownership & audit
                    EndUserId = GetUserId(),
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false,

                    // (Optional) save original user block:
                    PersonalInfoText = dto.PersonalInfoText
                };

                // Profile image upload
                if (dto.ProfileImage != null)
                {
                    using var ms = new MemoryStream();
                    await dto.ProfileImage.CopyToAsync(ms);
                    portfolio.PortfolioImage = ms.ToArray();
                    portfolio.PortfolioImageName = dto.ProfileImage.FileName;
                    portfolio.PortfolioImageType = dto.ProfileImage.ContentType;
                }

                // Map Services via join entity
                portfolio.PortfolioServices = dto.ServiceIds
                    .Select(sid => new PortfolioService { ServiceId = sid })
                    .ToList();

                // Map Projects
                portfolio.Projects = dto.Projects.Select(pdto =>
                {
                    var proj = new Project
                    {
                        ProjectName = pdto.ProjectName,
                        ProjectDescription = pdto.ProjectDescription,
                        StartDate = pdto.StartDate,
                        EndDate = pdto.EndDate,
                        ProjectLink = pdto.ProjectLink,
                        ServiceId = pdto.ServiceId ?? default,
                        CustomServiceName = pdto.CustomServiceName,
                        Portfolio = portfolio
                    };

                    if (pdto.ProjectImage != null)
                    {
                        using var pms = new MemoryStream();
                        pdto.ProjectImage.CopyTo(pms);
                        proj.ProjectFile = pms.ToArray();
                        proj.ProjectFileName = pdto.ProjectImage.FileName;
                        proj.ProjectFileType = pdto.ProjectImage.ContentType;
                    }

                    return proj;
                }).ToList();

                // Save
                _portfolioRepository.Create(portfolio);

                return RedirectToAction(nameof(Details), new { id = portfolio.PortfolioId });
            }
            catch (Exception x)
            {
                // Optionally add logging here!
                throw;
            }
        }


        // GET: /Portfolio/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var portfolio = _portfolioRepository.GetPortfolioById(id);
            if (portfolio == null || portfolio.EndUserId != GetUserId())
                return NotFound();

            var dto = new PortfolioUpdateDTO
            {
                PortfolioId = portfolio.PortfolioId,
                HasProfileImage = portfolio.PortfolioImage != null,
                PersonalInfoText = portfolio.PersonalInfoText ?? "", // Use saved freeform text
                ServiceIds = portfolio.PortfolioServices.Select(ps => ps.ServiceId).ToList(),
                Projects = portfolio.Projects.Select(p => new ProjectUpdateDTO
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    ProjectLink = p.ProjectLink,
                    ServiceId = p.ServiceId,
                    CustomServiceName = p.CustomServiceName,
                    HasProjectImage = p.ProjectFile != null,
                    // ProjectImage left null for edit
                }).ToList()
            };

            ViewBag.Services = _context.Services.ToList();
            return View(dto);
        }

        // POST: /Portfolio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PortfolioUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = _context.Services.ToList();
                return View(dto);
            }

            var portfolio = _portfolioRepository.GetPortfolioById(dto.PortfolioId);
            if (portfolio == null || portfolio.EndUserId != GetUserId())
                return NotFound();

            // 1. AI-enhance: Pass PersonalInfoText (merged bio/info block)
            var aiResult = await _parser.ParsePortfolioPersonalInfoAsync(dto.PersonalInfoText);

            // 2. Overwrite personal info with AI result
            portfolio.FirstName = aiResult.FirstName;
            portfolio.SecondName = aiResult.SecondName;
            portfolio.ThirdName = aiResult.ThirdName;
            portfolio.LastName = aiResult.LastName;
            portfolio.Email = aiResult.Email;
            portfolio.PhoneNumber = aiResult.PhoneNumber;
            portfolio.LinkedInLink = aiResult.LinkedInLink;
            portfolio.GithubLink = aiResult.GithubLink;
            portfolio.FacebookLink = aiResult.FacebookLink;
            portfolio.InstagramLink = aiResult.InstagramLink;
            portfolio.Address = aiResult.Address;
            portfolio.DateOfBirth = aiResult.DateOfBirth;
            portfolio.Summary = aiResult.EnhancedSummary;

            // 3. Save freeform block for next edit
            portfolio.PersonalInfoText = dto.PersonalInfoText;

            // 4. Profile image
            if (dto.ProfileImage != null)
            {
                using var ms = new MemoryStream();
                await dto.ProfileImage.CopyToAsync(ms);
                portfolio.PortfolioImage = ms.ToArray();
                portfolio.PortfolioImageName = dto.ProfileImage.FileName;
                portfolio.PortfolioImageType = dto.ProfileImage.ContentType;
            }

            // 5. Refresh Service links
            portfolio.PortfolioServices.Clear();
            foreach (var sid in dto.ServiceIds)
                portfolio.PortfolioServices.Add(new PortfolioService
                {
                    PortfolioId = portfolio.PortfolioId,
                    ServiceId = sid
                });

            // 6. Projects logic (same as before)
            var existingProjectsDict = portfolio.Projects.ToDictionary(p => p.ProjectId);
            portfolio.Projects.Clear();

            foreach (var pdto in dto.Projects)
            {
                Project proj;
                if (pdto.ProjectId.HasValue && existingProjectsDict.TryGetValue(pdto.ProjectId.Value, out var oldProject))
                {
                    proj = new Project
                    {
                        ProjectId = oldProject.ProjectId,
                        ProjectName = pdto.ProjectName,
                        ProjectDescription = pdto.ProjectDescription,
                        StartDate = pdto.StartDate,
                        EndDate = pdto.EndDate,
                        ProjectLink = pdto.ProjectLink,
                        ServiceId = pdto.ServiceId,
                        CustomServiceName = pdto.CustomServiceName,
                        Portfolio = portfolio,
                        ProjectFile = oldProject.ProjectFile,
                        ProjectFileName = oldProject.ProjectFileName,
                        ProjectFileType = oldProject.ProjectFileType
                    };
                    if (pdto.ProjectImage != null)
                    {
                        using var pms = new MemoryStream();
                        await pdto.ProjectImage.CopyToAsync(pms);
                        proj.ProjectFile = pms.ToArray();
                        proj.ProjectFileName = pdto.ProjectImage.FileName;
                        proj.ProjectFileType = pdto.ProjectImage.ContentType;
                    }
                }
                else
                {
                    proj = new Project
                    {
                        ProjectName = pdto.ProjectName,
                        ProjectDescription = pdto.ProjectDescription,
                        StartDate = pdto.StartDate,
                        EndDate = pdto.EndDate,
                        ProjectLink = pdto.ProjectLink,
                        ServiceId = pdto.ServiceId,
                        CustomServiceName = pdto.CustomServiceName,
                        Portfolio = portfolio
                    };
                    if (pdto.ProjectImage != null)
                    {
                        using var pms = new MemoryStream();
                        await pdto.ProjectImage.CopyToAsync(pms);
                        proj.ProjectFile = pms.ToArray();
                        proj.ProjectFileName = pdto.ProjectImage.FileName;
                        proj.ProjectFileType = pdto.ProjectImage.ContentType;
                    }
                }
                portfolio.Projects.Add(proj);
            }

            portfolio.ModifiedDate = DateTime.UtcNow;
            _portfolioRepository.Update(portfolio);

            return RedirectToAction(nameof(Details), new { id = portfolio.PortfolioId });
        }


        // GET: /Portfolio/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            var portfolio = _portfolioRepository.GetPortfolioById(id);
            if (portfolio == null) return NotFound();
            return View(portfolio);
        }

        [HttpGet]
        public IActionResult Details2(int id)
        {
            var portfolio = _portfolioRepository.GetPortfolioById(id);
            if (portfolio == null) return NotFound();
            return View(portfolio);
        }

        // GET: /Portfolio/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var portfolio = _portfolioRepository.GetPortfolioById(id);
            if (portfolio == null) return NotFound();
            return View(portfolio);
        }

        // POST: /Portfolio/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int PortfolioId)
        {
            _portfolioRepository.Delete(PortfolioId);
            TempData["SuccessMessage"] = "Portfolio deleted successfully!";
            return RedirectToAction("Index");
        }



        // GET: /Portfolio/ProfileImage/5
        [HttpGet]
        public IActionResult Image(string entity, int id)
        {
            if (entity.Equals("portfolio", StringComparison.OrdinalIgnoreCase))
            {
                // Portfolio image
                var portfolio = _portfolioRepository.GetPortfolioById(id);
                if (portfolio?.PortfolioImage == null) return NotFound();
                return File(
                    portfolio.PortfolioImage,
                    portfolio.PortfolioImageType!,
                    enableRangeProcessing: true
                );
            }
            else if (entity.Equals("project", StringComparison.OrdinalIgnoreCase))
            {
                // Project image
                var project = _context.Projects
                    .SingleOrDefault(p => p.ProjectId == id);
                if (project?.ProjectFile == null) return NotFound();
                return File(
                    project.ProjectFile,
                    project.ProjectFileType!,
                    enableRangeProcessing: true
                );
            }
            else if (entity.Equals("service", StringComparison.OrdinalIgnoreCase))
            {
                // Service image
                var service = _context.Services
                    .SingleOrDefault(s => s.ServiceId == id);
                if (service?.ServiceImage == null) return NotFound();
                return File(
                    service.ServiceImage,
                    service.ServiceImageType ?? "image/png", // fallback for null MIME type
                    enableRangeProcessing: true
                );
            }
            return NotFound();
        }

    }
}
