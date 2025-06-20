namespace CVisionary.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Service? Service { get; set; }
        public short ServiceId { get; set; }

        public string ?ProjectLink { get; set; }

        public Portfolio Portfolio { get; set; }

        public int PortfolioId { get; set; }

        public string? CustomServiceName { get; set; }

        public string ?ProjectFileName { get; set; }
        public string ?ProjectFileType { get; set; }
        public byte[] ?ProjectFile { get; set; }


    }
}
