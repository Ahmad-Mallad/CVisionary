using System.ComponentModel.DataAnnotations;

namespace CVisionary.DTOs
{
    public class PortfolioCreateDTO
    {
        // 1) The user’s raw bio (to be AI-enhanced)
        [Required]
        public string PersonalInfoText { get; set; } 

        // 2a) User’s own profile picture
        public IFormFile? ProfileImage { get; set; }

        // 3) Select existing services by ID
        [Required]
        public List<short> ServiceIds { get; set; } = new();

        // 4) One-to-many Projects
        [Required]
        public List<ProjectCreateDto> Projects { get; set; } = new();
    }

}
