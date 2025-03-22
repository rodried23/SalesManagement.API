using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using SalesManagement.Domain.Interfaces.Repositories;
using SalesManagement.Infrastructure.Data.Context;

namespace SalesManagement.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SalesDbContext _context;
        private IDbContextTransaction _transaction;

        public ISaleRepository SaleRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IBranchRepository BranchRepository { get; }

        public UnitOfWork(
            SalesDbContext context,
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IBranchRepository branchRepository)
        {
            _context = context;
            SaleRepository = saleRepository;
            ProductRepository = productRepository;
            CustomerRepository = customerRepository;
            BranchRepository = branchRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}