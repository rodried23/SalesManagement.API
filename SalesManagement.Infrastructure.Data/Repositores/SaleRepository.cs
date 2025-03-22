using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Domain.Entities;
using SalesManagement.Domain.Interfaces.Repositories;
using SalesManagement.Infrastructure.Data.Context;

namespace SalesManagement.Infrastructure.Data.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        public SaleRepository(SalesDbContext context) : base(context)
        {
        }

        public override async Task<Sale?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sale?> GetByNumberAsync(int saleNumber)
        {
            return await _dbSet
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
        }

        public async Task<IEnumerable<Sale>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _dbSet
                .Include(s => s.Items)
                .Where(s => s.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByBranchIdAsync(Guid branchId)
        {
            return await _dbSet
                .Include(s => s.Items)
                .Where(s => s.BranchId == branchId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(s => s.Items)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync();
        }

        public async Task<int> GenerateSaleNumberAsync()
        {
            if (!await _dbSet.AnyAsync())
                return 1;

            return await _dbSet.MaxAsync(s => s.SaleNumber) + 1;
        }
    }
}