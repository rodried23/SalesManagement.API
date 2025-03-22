using System;
using MediatR;
using SalesManagement.Application.DTOs;

namespace SalesManagement.Application.Queries.Products
{
    public class GetProductQuery : IRequest<ProductDTO>
    {
        public Guid Id { get; set; }
    }
}