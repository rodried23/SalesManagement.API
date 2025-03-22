using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Sales;
using SalesManagement.Domain.Interfaces.Repositories;

namespace SalesManagement.Application.Queries.Handlers.Sales
{
    public class GetSaleQueryHandler : IRequestHandler<GetSaleQuery, SaleDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSaleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SaleDTO> Handle(GetSaleQuery request, CancellationToken cancellationToken)
        {
            var sale = await _unitOfWork.SaleRepository.GetByIdAsync(request.Id);
            return _mapper.Map<SaleDTO>(sale);
        }
    }
}