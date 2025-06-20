using CVisionary.Models;

namespace CVisionary.Repositories.Interfaces
{
    public interface IPortfolioRepository
    {
        public List<Portfolio> GetAllPortfolios(string UserId);

        public Portfolio GetPortfolioById(int Id);

        public void Create(Portfolio portfolio);

        public void Update(Portfolio portfolio);

        public void Delete(int Id);
    }
}
