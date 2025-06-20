using CVisionary.Data;
using CVisionary.Models;
using CVisionary.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVisionary.Repositories.Repos
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly ApplicationDbContext _context;

        public ResumeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Resume> GetAllResumes(string UserId)
        {
            return _context.Resumes
                 .Include(x => x.Certificates)
                 .Include(x => x.Educations)
                 .Include(x => x.Experiences)
                 .Include(x => x.Languages)
                 .Include(x => x.Skills)
                 .Where(x => x.EndUserId == UserId && !x.IsDeleted)
                 .ToList();
        }

        public Resume GetResumeById(int id)
        {
            return _context.Resumes
                .Include(r => r.Certificates)
                .Include(r => r.Educations)
                .Include(r => r.Experiences)
                .Include(r => r.Languages)
                .Include(r => r.Skills)
                .FirstOrDefault(r => r.ResumeId == id);
        }
        public void Create(Resume resume)
        {
            _context.Resumes.Add(resume);
            _context.SaveChanges();
        }

        public void Update(Resume resume)
        {
            _context.Resumes.Update(resume);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var resume = _context.Resumes.Find(id);
            if (resume != null)
            {
                resume.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}
