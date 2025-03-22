using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SalesManagement.Application.Commands.Products;
using SalesManagement.Domain.Entities;
using SalesManagement.Domain.Interfaces.Repositories;

namespace SalesManagement.Application.Commands.Handlers.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(
                request.Name,
                request.Description,
                request.Category,
                request.Price,
                request.ImageUrl);

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }
    }
}