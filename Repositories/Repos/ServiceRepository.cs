using CVisionary.Data;
using CVisionary.Models;
using CVisionary.Repositories.Interfaces;

namespace CVisionary.Repositories.Repos
{
    public class ServiceRepository:IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Service> GetAll() => _context.Services.ToList();

        public Service GetById(short id) => _context.Services.SingleOrDefault(x=>x.ServiceId==id);

        public void Add(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public void Update(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
        }

        public void Delete(short id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
        }

    }
}
