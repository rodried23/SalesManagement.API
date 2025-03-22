using System;
using System.Threading.Tasks;
using MediatR;
using SalesManagement.Application.Commands.Sales;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Sales;
using SalesManagement.Application.Services.Interfaces;

namespace SalesManagement.Application.Services.Implementations
{
    public class SaleService : ISaleService
    {
        private readonly IMediator _mediator;

        public SaleService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<PaginatedResult<SaleDTO>> GetAllSalesAsync(GetAllSalesQuery query)
        {
            return await _mediator.Send(query);
        }

        public async Task<SaleDTO> GetSaleByIdAsync(Guid id)
        {
            return await _mediator.Send(new GetSaleQuery { Id = id });
        }

        public async Task<Guid> CreateSaleAsync(CreateSaleCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<bool> UpdateSaleAsync(UpdateSaleCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<bool> CancelSaleAsync(Guid id)
        {
            return await _mediator.Send(new CancelSaleCommand { Id = id });
        }
    }
}