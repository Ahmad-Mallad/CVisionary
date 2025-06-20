namespace CVisionary.Models
{
    public class Resume : PersonalInfo
    {
        public int ResumeId { get; set; }

        public string? CreatedDate { get; set; }
        public string? ModifiedDate { get; set; }

        public List<Education> Educations { get; set; }

        public List<Experience> Experiences { get; set; }

        public List<Skill> Skills { get; set; }

        public List<Language> Languages { get; set; }

        public List<Certificate> Certificates { get; set; }

        public EndUser EndUser { get; set; }
        public string EndUserId { get; set; }

        public bool IsDeleted { get; set; }

        public string? ResumeFileName { get; set; }
        public string? ResumeFileType { get; set; }
        public byte[]? ResumeFile { get; set; }


        // Raw input fields
        public string? PersonalInfoText { get; set; }
        public string? SummaryText { get; set; }
        public string? EducationText { get; set; }
        public string? ExperienceText { get; set; }
        public string? SkillsText { get; set; }
        public string? CertificatesText { get; set; }
        public string? LanguagesText { get; set; }

        // New merged fields
        public string? PersonalSummary { get; set; }
        public string? ProfessionalHistory { get; set; }
        public string? ProfessionalSkills { get; set; }

    }
}
