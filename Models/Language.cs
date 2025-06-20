namespace CVisionary.Models
{
    public class Language
    {
        public short LanguageId { get; set; }

        public string LanguageName { get; set; }

        public string ?Level { get; set; }

        public Resume Resume { get; set; }
        public int ResumeId { get; set; }

    }
}
