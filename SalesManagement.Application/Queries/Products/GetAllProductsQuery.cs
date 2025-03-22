using MediatR;
using SalesManagement.Application.DTOs;

namespace SalesManagement.Application.Queries.Products
{
    public class GetAllProductsQuery : IRequest<PaginatedResult<ProductDTO>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Name";
        public string SortDirection { get; set; } = "asc";
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Category { get; set; }
        public bool? IsActive { get; set; }
    }
}