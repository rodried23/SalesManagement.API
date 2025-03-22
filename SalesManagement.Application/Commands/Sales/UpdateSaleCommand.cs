using System;
using System.Collections.Generic;
using MediatR;

namespace SalesManagement.Application.Commands.Sales
{
    public class UpdateSaleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public List<UpdateSaleItemCommand> Items { get; set; } = new List<UpdateSaleItemCommand>();
    }

    public class UpdateSaleItemCommand
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}