using System.ComponentModel.DataAnnotations;

namespace CVisionary.DTOs
{
    public class ServiceUpdateDTO
    {
        [Required]
        public short ServiceId { get; set; }

        [Required]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [Display(Name = "Description")]
        public string? ServiceDescription { get; set; }

        [Display(Name = "Service Image")]
        public IFormFile? ServiceImageFile { get; set; }  // For image upload

        // For displaying the old image (e.g., in the edit view)
        public bool HasExistingImage { get; set; }
    }
}
