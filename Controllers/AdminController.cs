using CVisionary.Data;
using CVisionary.Models;
using CVisionary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CVisionary.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            // 1. Statistics
            var totalUsers = _context.Users.Count();
            var totalResumes = _context.Resumes.Count();
            var totalPortfolios = _context.Portfolios.Count();

            // 2. Charts: group by month (last 6 months)
            // Get last 6 months as "yyyy-MM"
            var last6Months = Enumerable.Range(0, 6)
                .Select(i => DateTime.UtcNow.AddMonths(-i))
                .OrderBy(d => d)
                .Select(d => d.ToString("yyyy-MM"))
                .ToList();

            // Resumes per month
            var resumesPerMonthDict = _context.Resumes
                .Where(r => r.CreatedDate != null)
                .AsEnumerable()
                .GroupBy(r => r.CreatedDate!.Substring(0, 7)) // "yyyy-MM"
                .ToDictionary(g => g.Key, g => g.Count());

            // Portfolios per month
            var portfoliosPerMonthDict = _context.Portfolios
                .Where(p => p.CreatedDate != null)
                .AsEnumerable()
                .GroupBy(p => p.CreatedDate!.Value.ToString("yyyy-MM"))
                .ToDictionary(g => g.Key, g => g.Count());

            // 3. Latest
            var latestResumes = _context.Resumes
                .OrderByDescending(r => r.CreatedDate)
                .Take(5)
                .ToList();

            var latestPortfolios = _context.Portfolios
                .OrderByDescending(p => p.CreatedDate)
                .Take(5)
                .ToList();

            // 4. Prepare viewmodel with chart lists in same order
            var vm = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalResumes = totalResumes,
                TotalPortfolios = totalPortfolios,
                ChartLabels = last6Months,
                ResumesPerMonth = last6Months.Select(m => resumesPerMonthDict.ContainsKey(m) ? resumesPerMonthDict[m] : 0).ToList(),
                PortfoliosPerMonth = last6Months.Select(m => portfoliosPerMonthDict.ContainsKey(m) ? portfoliosPerMonthDict[m] : 0).ToList(),
                LatestResumes = latestResumes,
                LatestPortfolios = latestPortfolios,
            };

            return View(vm);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AllResumes()
        {
            var allResumes = _context.Resumes
                .OrderByDescending(r => r.CreatedDate).ToList();
            return View(allResumes);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AllPortfolios()
        {
            var allPortfolios = _context.Portfolios.Include(r => r.EndUser)
                .OrderByDescending(p => p.CreatedDate).ToList();
            return View(allPortfolios);
        }


    }
}
