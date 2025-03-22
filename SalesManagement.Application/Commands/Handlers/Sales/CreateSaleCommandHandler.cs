using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SalesManagement.Application.Commands.Sales;
using SalesManagement.Domain.Entities;
using SalesManagement.Domain.Interfaces.Repositories;

namespace SalesManagement.Application.Commands.Handlers.Sales
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSaleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            // Generate sale number
            var saleNumber = await _unitOfWork.SaleRepository.GenerateSaleNumberAsync();

            // Create new sale
            var sale = new Sale(
                saleNumber,
                request.CustomerId,
                request.CustomerName,
                request.BranchId,
                request.BranchName);

            // Add items to sale
            foreach (var item in request.Items)
            {
                sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
            }

            // Save sale
            await _unitOfWork.SaleRepository.AddAsync(sale);
            await _unitOfWork.SaveChangesAsync();

            return sale.Id;
        }
    }
}