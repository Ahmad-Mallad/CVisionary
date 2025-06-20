using System.ComponentModel.DataAnnotations;

namespace CVisionary.DTOs
{
    public class ProjectCreateDto
    {
        [Required]
        public string ProjectName { get; set; } 

        [Required]
        public string ProjectDescription { get; set; } 

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? ProjectLink { get; set; }

       
        public IFormFile? ProjectImage { get; set; }

        
        public short? ServiceId { get; set; }

        
        public string? CustomServiceName { get; set; }
    }
}
