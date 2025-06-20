namespace CVisionary.Models
{
    public class PortfolioService
    {
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public short ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
