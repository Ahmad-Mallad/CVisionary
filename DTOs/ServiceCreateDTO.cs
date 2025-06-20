using System.ComponentModel.DataAnnotations;

namespace CVisionary.DTOs
{
    public class ServiceCreateDTO
    {
        [Required]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [Display(Name = "Description")]
        public string? ServiceDescription { get; set; }

        [Display(Name = "Service Image")]
        public IFormFile? ServiceImageFile { get; set; }  // For image upload
    }
}
