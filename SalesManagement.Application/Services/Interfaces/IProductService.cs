using System;
using System.Threading.Tasks;
using SalesManagement.Application.Commands.Products;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Products;

namespace SalesManagement.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(GetAllProductsQuery query);
        Task<ProductDTO> GetProductByIdAsync(Guid id);
        Task<Guid> CreateProductAsync(CreateProductCommand command);
        Task<bool> UpdateProductAsync(UpdateProductCommand command);
        Task<bool> DeleteProductAsync(Guid id);
    }
}