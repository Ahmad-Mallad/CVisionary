using CVisionary.Data;
using CVisionary.DTOs;
using CVisionary.Models;
using CVisionary.Repositories.Interfaces;
using CVisionary.Repositories.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CVisionary.Controllers
{
    [Authorize]
    public class ResumeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IResumeRepository _resumeRepository;
        private readonly ICVParser _parser;

        public ResumeController(ApplicationDbContext context, IResumeRepository resumeRepository, ICVParser parser)
        {
            _context = context;
            _resumeRepository = resumeRepository;
            _parser = parser;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Index()
        {
            var UserId = GetUserId();
            ViewBag.Resumes = _resumeRepository.GetAllResumes(UserId);
            return View();
        }

        [HttpGet]
       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ResumeCreateDTO resumeCreateDTO)
        {
            if (!ModelState.IsValid)
                return View(resumeCreateDTO);

            // 1. Combine all inputs into one raw string
            string rawText = $@"
            {resumeCreateDTO.PersonalSummary}

            History:
            {resumeCreateDTO.ProfessionalHistory}
            
            Skills:
            {resumeCreateDTO.ProfessionalSkills}

            ";

            // 2. Send to AI Parser
            Resume parsedResume = await _parser.ParseCvAsync(rawText);

            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(parsedResume.FirstName) || parsedResume.FirstName == "Not Provided")
                errors.Add("First name is required.");
            if (string.IsNullOrWhiteSpace(parsedResume.LastName) || parsedResume.LastName == "Not Provided")
                errors.Add("Last name is required.");
            if (string.IsNullOrWhiteSpace(parsedResume.Email) || parsedResume.Email == "Not Provided")
                errors.Add("Email is required.");
            // (Add any other checks you want...)

            if (errors.Count > 0)
            {
                ViewBag.AIErrors = errors;
                return View(resumeCreateDTO); // re-display form, show errors
            }

            // 3. Set user ID and other properties
            parsedResume.EndUserId = GetUserId();

            // (Optional) Store raw input
            parsedResume.PersonalSummary = resumeCreateDTO.PersonalSummary;
            parsedResume.ProfessionalHistory = resumeCreateDTO.ProfessionalHistory;
            parsedResume.ProfessionalSkills = resumeCreateDTO.ProfessionalSkills;

            parsedResume.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd");

            // 4. Save to database
            _resumeRepository.Create(parsedResume);

            // 5. Redirect to a resume details page
            return View("ParsedResume", parsedResume);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var oldResume = _resumeRepository.GetResumeById(Id);

            if (oldResume == null)
                return NotFound();

            var dto = new ResumeUpdateDTO
            {
               ResumeId=oldResume.ResumeId,
                PersonalSummary = oldResume.PersonalSummary,
                ProfessionalHistory = oldResume.ProfessionalHistory,
                ProfessionalSkills = oldResume.ProfessionalSkills,
                
            };

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ResumeUpdateDTO newResume)
        {
            // Step 1: Get the existing resume
            var oldResume = _resumeRepository.GetResumeById(newResume.ResumeId);
            if (oldResume == null)
                return NotFound();

            // Step 2: Combine updated inputs into raw text for AI
            string rawText = $@"
            {newResume.PersonalSummary}

            Summary:
            {newResume.ProfessionalHistory}

            Education:
            {newResume.ProfessionalSkills}

      
            ";

            // Step 3: Parse the updated content using AI
            Resume parsedResume = await _parser.ParseCvAsync(rawText);

            // Step 4: Update the old resume fields (raw input)
            oldResume.PersonalSummary = newResume.PersonalSummary;
            oldResume.ProfessionalHistory = newResume.ProfessionalHistory;
            oldResume.ProfessionalSkills = newResume.ProfessionalSkills;
      

            // Update structured parsed fields
        
            oldResume.PersonalSummary = parsedResume.PersonalSummary;
            oldResume.ProfessionalHistory = parsedResume.ProfessionalHistory;
            oldResume.ProfessionalSkills = parsedResume.ProfessionalSkills;
            

            _resumeRepository.Update(oldResume);

            return RedirectToAction("details", new { id = oldResume.ResumeId });

        }

        public IActionResult Details(int id)
        {
            var resume = _resumeRepository.GetResumeById(id);
            if (resume == null)
                return NotFound();

            return View("ParsedResume", resume); // Reuse your existing view
        }

        public IActionResult Delete(int id)
        {
            var resume = _resumeRepository.GetResumeById(id);
            if (resume==null)
            {
                return NotFound();
            }
            return View(resume);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int ResumeId)
        {
            _resumeRepository.Delete(ResumeId);
            return RedirectToAction("Index");
        }

        public IActionResult DownloadPdf(int id)
        {
            var resume = _resumeRepository.GetResumeById(id);
            if (resume == null)
                return NotFound();

            return new Rotativa.AspNetCore.ViewAsPdf("Pdf", resume)
            {
                FileName = $"{resume.FirstName}_{resume.LastName}_Resume.pdf",
                PageMargins = new Rotativa.AspNetCore.Options.Margins(20, 20, 20, 20)
            };
        }


    }
}
