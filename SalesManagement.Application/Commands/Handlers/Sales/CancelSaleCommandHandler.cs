using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SalesManagement.Application.Commands.Sales;
using SalesManagement.Domain.Exceptions;
using SalesManagement.Domain.Interfaces.Repositories;

namespace SalesManagement.Application.Commands.Handlers.Sales
{
    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelSaleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _unitOfWork.SaleRepository.GetByIdAsync(request.Id);
            if (sale == null)
                throw new DomainException("Compra não encontrada!");

            sale.Cancel();

            await _unitOfWork.SaleRepository.UpdateAsync(sale);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}