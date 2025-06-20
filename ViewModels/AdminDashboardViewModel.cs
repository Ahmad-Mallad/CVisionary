using CVisionary.Models;

namespace CVisionary.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalResumes { get; set; }
        public int TotalPortfolios { get; set; }

        public List<string> ChartLabels { get; set; } = new();
        public List<int> ResumesPerMonth { get; set; } = new();
        public List<int> PortfoliosPerMonth { get; set; } = new();

        public List<Resume> LatestResumes { get; set; } = new();
        public List<Portfolio> LatestPortfolios { get; set; } = new();
    }
}
