namespace CVisionary.Models
{
    public class EndUser:Person
    {
        public List<Resume> Resumes { get; set; }
        public List<Portfolio> Portfolios { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
