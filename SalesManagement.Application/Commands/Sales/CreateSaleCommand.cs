using System;
using System.Collections.Generic;
using MediatR;

namespace SalesManagement.Application.Commands.Sales
{
    public class CreateSaleCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public List<SaleItemCommand> Items { get; set; } = new List<SaleItemCommand>();
    }

    public class SaleItemCommand
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}