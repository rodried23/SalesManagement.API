using System;
using System.Threading.Tasks;

namespace SalesManagement.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ISaleRepository SaleRepository { get; }
        IProductRepository ProductRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IBranchRepository BranchRepository { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}