using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesManagement.Domain.Entities;

namespace SalesManagement.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
    }
}