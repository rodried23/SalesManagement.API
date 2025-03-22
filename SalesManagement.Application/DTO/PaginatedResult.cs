using System.Collections.Generic;

namespace SalesManagement.Application.DTOs
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public PaginatedResult(IEnumerable<T> data, int totalItems, int currentPage, int pageSize)
        {
            Data = data;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (totalItems + pageSize - 1) / pageSize;
        }
    }
}