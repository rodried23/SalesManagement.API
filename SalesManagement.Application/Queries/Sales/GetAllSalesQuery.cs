using MediatR;
using SalesManagement.Application.DTOs;

namespace SalesManagement.Application.Queries.Sales
{
    public class GetAllSalesQuery : IRequest<PaginatedResult<SaleDTO>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "SaleDate";
        public string SortDirection { get; set; } = "desc";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? BranchId { get; set; }
    }
}