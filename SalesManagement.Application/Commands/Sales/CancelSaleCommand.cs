using System;
using MediatR;

namespace SalesManagement.Application.Commands.Sales
{
    public class CancelSaleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}