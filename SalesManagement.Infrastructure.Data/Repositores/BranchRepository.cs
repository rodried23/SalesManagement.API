using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Domain.Entities;
using SalesManagement.Domain.Interfaces.Repositories;
using SalesManagement.Infrastructure.Data.Context;

namespace SalesManagement.Infrastructure.Data.Repositories
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        public BranchRepository(SalesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Branch>> GetActiveBranchesAsync()
        {
            return await _dbSet
                .Where(b => b.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Branch>> GetByStateAsync(string state)
        {
            return await _dbSet
                .Where(b => b.State.ToLower() == state.ToLower())
                .ToListAsync();
        }
    }
}