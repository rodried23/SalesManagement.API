using System;
using System.Threading.Tasks;
using SalesManagement.Application.Commands.Sales;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Sales;

namespace SalesManagement.Application.Services.Interfaces
{
    public interface ISaleService
    {
        Task<PaginatedResult<SaleDTO>> GetAllSalesAsync(GetAllSalesQuery query);
        Task<SaleDTO> GetSaleByIdAsync(Guid id);
        Task<Guid> CreateSaleAsync(CreateSaleCommand command);
        Task<bool> UpdateSaleAsync(UpdateSaleCommand command);
        Task<bool> CancelSaleAsync(Guid id);
    }
}