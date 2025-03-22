using System.Collections.Generic;
using System.Threading.Tasks;
using SalesManagement.Domain.Entities;

namespace SalesManagement.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(string email);
        Task<IEnumerable<Customer>> SearchByNameAsync(string name);
    }
}