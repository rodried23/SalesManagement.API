using System.Collections.Generic;
using System.Threading.Tasks;
using SalesManagement.Domain.Entities;

namespace SalesManagement.Domain.Interfaces.Repositories
{
    public interface IBranchRepository : IRepository<Branch>
    {
        Task<IEnumerable<Branch>> GetActiveBranchesAsync();
        Task<IEnumerable<Branch>> GetByStateAsync(string state);
    }
}