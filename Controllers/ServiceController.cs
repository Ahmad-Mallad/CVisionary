using CVisionary.DTOs;
using CVisionary.Models;
using CVisionary.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVisionary.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepo;

        public ServiceController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var services = _serviceRepo.GetAll();
            return View(services);
        }

        // GET: AdminService/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ServiceCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var service = new Service
            {
                ServiceName = dto.ServiceName,
                ServiceDescription = dto.ServiceDescription,
            };

            if (dto.ServiceImageFile != null)
            {
                using var ms = new MemoryStream();
                await dto.ServiceImageFile.CopyToAsync(ms);
                service.ServiceImage = ms.ToArray();
                service.ServiceImageName = dto.ServiceImageFile.FileName;
                service.ServiceImageType = dto.ServiceImageFile.ContentType;
            }

            _serviceRepo.Add(service);
            return RedirectToAction("Index");
        }

        // GET: AdminService/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(short id)
        {
            var service = _serviceRepo.GetById(id);
            if (service == null)
                return NotFound();

            var dto = new ServiceUpdateDTO
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                ServiceDescription = service.ServiceDescription,
                HasExistingImage = service.ServiceImage != null
            };

            return View(dto);
        }

        // POST: AdminService/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ServiceUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var service = _serviceRepo.GetById(dto.ServiceId);
            if (service == null)
                return NotFound();

            service.ServiceName = dto.ServiceName;
            service.ServiceDescription = dto.ServiceDescription;

            // Replace image only if a new one was uploaded
            if (dto.ServiceImageFile != null)
            {
                using var ms = new MemoryStream();
                await dto.ServiceImageFile.CopyToAsync(ms);
                service.ServiceImage = ms.ToArray();
                service.ServiceImageName = dto.ServiceImageFile.FileName;
                service.ServiceImageType = dto.ServiceImageFile.ContentType;
            }
            // Otherwise, keep existing image

            _serviceRepo.Update(service);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(short id)
        {
            var service = _serviceRepo.GetById(id);
            if (service == null)
                return NotFound();
            return View(service);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(short ServiceId)
        {
            _serviceRepo.Delete(ServiceId);
            return RedirectToAction("Index");
        }

        // Optional: image serving endpoint for display in table/view
        public IActionResult ServiceImage(short id)
        {
            var service = _serviceRepo.GetById(id);
            if (service?.ServiceImage == null)
                return NotFound();
            return File(service.ServiceImage, service.ServiceImageType ?? "image/png");
        }
    }
}
