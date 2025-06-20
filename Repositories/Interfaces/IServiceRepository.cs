using CVisionary.Models;

namespace CVisionary.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        List<Service> GetAll();
        Service GetById(short id);
        void Add(Service service);
        void Update(Service service);
        void Delete(short id);
    }
}
