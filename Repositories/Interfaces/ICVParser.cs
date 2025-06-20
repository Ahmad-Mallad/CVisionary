using CVisionary.Models;

namespace CVisionary.Repositories.Interfaces
{
    public interface ICVParser
    {
        Task<Resume> ParseCvAsync(string rawText);

    }
}
