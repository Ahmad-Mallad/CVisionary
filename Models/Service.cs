namespace CVisionary.Models
{
    public class Service
    {
        public short ServiceId { get; set; }

        public string? ServiceName { get; set; }

        public string? ServiceDescription { get; set; }

        public List<Project> Projects { get; set; }

        public List<PortfolioService> PortfolioServices { get; set; }

        public byte[]? ServiceImage { get; set; }           // Store the image bytes
        public string? ServiceImageName { get; set; }       // Store the original file name (optional)
        public string? ServiceImageType { get; set; }       // Store MIME type (image/png, etc.)

    }
}
