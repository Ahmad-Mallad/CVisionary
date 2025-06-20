using CVisionary.Data;
using CVisionary.Models;
using CVisionary.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVisionary.Repositories.Repos
{
    public class PortfolioRepository : IPortfolioRepository
    {

        private readonly ApplicationDbContext _context;

        public PortfolioRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Portfolio> GetAllPortfolios(string UserId)
        {
            return _context.Portfolios
                 .Include(x => x.Services)
                 .Include(x => x.Projects)
                 .Include(x=>x.PortfolioServices)
                 .Where(x => x.EndUserId == UserId && ! x.IsDeleted)
                 .ToList();
        }

        public Portfolio GetPortfolioById(int id)
        {
            return _context.Portfolios
              .Include(p => p.PortfolioServices)
                .ThenInclude(ps => ps.Service)
              .Include(p => p.Projects)
                .ThenInclude(pr => pr.Service)
              .SingleOrDefault(p => p.PortfolioId == id);
        }

        public void Create(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();
        }

        public void Update(Portfolio portfolio)
        {
            _context.Portfolios.Update(portfolio);
            _context.SaveChanges();
        }

        public void Delete(int Id)
        {
            var portfolio = _context.Portfolios.Find(Id);
            if (portfolio != null)
            {
                portfolio.IsDeleted = true;
                _context.SaveChanges();
            }

        }






    }
}
