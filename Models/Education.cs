namespace CVisionary.Models
{
    public class Education
    {
        public int EducationId { get; set; }
        public string CollegeName { get; set; }

        public string DegreeType { get; set; }

        public string? StartDate { get; set; } 
        public string? EndDate { get; set; }

        public string MajorName { get; set; }

        public double? GPA { get; set; }

        public Resume Resume { get; set; }
        public int ResumeId { get; set; }
    }
}
