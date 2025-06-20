namespace CVisionary.Models
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        public string? Title { get; set; }
        public string? CompanyName { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string Description { get; set; }

        public Resume Resume { get; set; }
        public int ResumeId { get; set; }

    }
}
