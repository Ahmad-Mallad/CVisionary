using System.ComponentModel.DataAnnotations;

namespace CVisionary.DTOs
{
    public class ProjectUpdateDTO
    {
        /// <summary>
        /// If editing an existing project, its ID; otherwise null for a new project
        /// </summary>
        public int? ProjectId { get; set; }

        [Required, Display(Name = "Project Name")]
        public string ProjectName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? ProjectDescription { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Core Service")]
        public short ServiceId { get; set; }

        [Display(Name = "Or Custom Service")]
        public string? CustomServiceName { get; set; }

        [Url, Display(Name = "Project Link")]
        public string? ProjectLink { get; set; }

        /// <summary>
        /// True if the project already has an image (for conditional rendering)
        /// </summary>
        public bool HasProjectImage { get; set; }

        /// <summary>
        /// New image file for this project (optional)
        /// </summary>
        [Display(Name = "Project Image")]
        public IFormFile? ProjectImage { get; set; }
    }

}
