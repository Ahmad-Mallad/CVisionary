using System.ComponentModel.DataAnnotations;

namespace CVisionary.DTOs
{
    public class ResumeUpdateDTO
    {
        public int ResumeId { get; set; }

        // 🔹 General Personal Info (freeform for AI to structure)
        public string PersonalSummary { get; set; }  // Name, contact, and a brief summary/about you

        // 2️⃣ Education, Experience, Certificates together
        [Required]
        public string ProfessionalHistory { get; set; } // All studies, jobs, certificates

        // 3️⃣ Skills & Languages together
        [Required]
        public string ProfessionalSkills { get; set; } // Skills and languages
    }

}
