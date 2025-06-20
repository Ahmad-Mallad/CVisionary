namespace CVisionary.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }

        public string ProviderName { get; set; }

        public string ?StartDate { get; set; }
        public string ?EndDate { get; set; }

        public string TopicName { get; set; }

        public double? GPA { get; set; }

        public Resume Resume { get; set; }
        public int ResumeId { get; set; }


    }
}
