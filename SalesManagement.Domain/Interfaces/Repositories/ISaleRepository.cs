using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesManagement.Domain.Entities;

namespace SalesManagement.Domain.Interfaces.Repositories
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<Sale> GetByNumberAsync(int saleNumber);
        Task<IEnumerable<Sale>> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Sale>> GetByBranchIdAsync(Guid branchId);
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GenerateSaleNumberAsync();
    }
}