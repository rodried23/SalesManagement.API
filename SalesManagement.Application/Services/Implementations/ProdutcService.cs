using System;
using System.Threading.Tasks;
using MediatR;
using SalesManagement.Application.Commands.Products;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Products;
using SalesManagement.Application.Services.Interfaces;
using SalesManagement.Domain.Interfaces.Repositories;

namespace SalesManagement.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(GetAllProductsQuery query)
        {
            return await _mediator.Send(query);
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            return await _mediator.Send(new GetProductQuery { Id = id });
        }

        public async Task<Guid> CreateProductAsync(CreateProductCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<bool> UpdateProductAsync(UpdateProductCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            product.Deactivate();
            await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}