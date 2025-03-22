using System;
using MediatR;
using SalesManagement.Application.DTOs;

namespace SalesManagement.Application.Queries.Sales
{
    public class GetSaleQuery : IRequest<SaleDTO>
    {
        public Guid Id { get; set; }
    }
}