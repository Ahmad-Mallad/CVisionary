using CVisionary.Models;

namespace CVisionary.Repositories.Interfaces
{

    public interface IPortfolioParser
    {
        Task<PortfolioInfoResult> ParsePortfolioPersonalInfoAsync(string personalInfoText);
    }
}
