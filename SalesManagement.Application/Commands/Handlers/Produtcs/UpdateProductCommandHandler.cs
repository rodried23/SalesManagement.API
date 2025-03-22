using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SalesManagement.Application.Commands.Products;
using SalesManagement.Domain.Exceptions;
using SalesManagement.Domain.Interfaces.Repositories;

namespace SalesManagement.Application.Commands.Handlers.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
            if (product == null)
                throw new DomainException("Produto não encontrado!");

            product.Update(
                request.Name,
                request.Description,
                request.Category,
                request.Price,
                request.ImageUrl);

            await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}