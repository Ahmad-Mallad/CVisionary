namespace CVisionary.Models
{
    public class Portfolio : PersonalInfo
    {
        public int PortfolioId { get; set; }
        public string ?PortfolioImageName { get; set; }
        public string ?PortfolioImageType { get; set; }
        public byte[] ?PortfolioImage { get; set; }

        public List<Service> Services { get; set; }
        public List<Project> Projects { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public string EndUserId { get; set; }
        public EndUser EndUser { get; set; }


        public List<PortfolioService> PortfolioServices { get; set; }

        public string ? PersonalInfoText { get; set; }



    }
}
