using CVisionary.Models;

namespace CVisionary.Repositories.Interfaces
{
    public interface IResumeRepository
    {
        public List<Resume> GetAllResumes(string UserId);

        public Resume GetResumeById(int Id);

        public void Create(Resume resume); 

        public void Update(Resume resume);

        public void Delete(int Id);


    }
}
