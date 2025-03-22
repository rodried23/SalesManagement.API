using System;
using MediatR;

namespace SalesManagement.Domain.Events
{
    public class SaleCreatedEvent : INotification
    {
        public Guid SaleId { get; }
        public int SaleNumber { get; }
        public Guid CustomerId { get; }
        public Guid BranchId { get; }
        public DateTime Timestamp { get; }

        public SaleCreatedEvent(Guid saleId, int saleNumber, Guid customerId, Guid branchId)
        {
            SaleId = saleId;
            SaleNumber = saleNumber;
            CustomerId = customerId;
            BranchId = branchId;
            Timestamp = DateTime.UtcNow;
        }
    }

    public class SaleModifiedEvent : INotification
    {
        public Guid SaleId { get; }
        public int SaleNumber { get; }
        public DateTime Timestamp { get; }

        public SaleModifiedEvent(Guid saleId, int saleNumber)
        {
            SaleId = saleId;
            SaleNumber = saleNumber;
            Timestamp = DateTime.UtcNow;
        }
    }

    public class SaleCanceledEvent : INotification
    {
        public Guid SaleId { get; }
        public int SaleNumber { get; }
        public DateTime Timestamp { get; }

        public SaleCanceledEvent(Guid saleId, int saleNumber)
        {
            SaleId = saleId;
            SaleNumber = saleNumber;
            Timestamp = DateTime.UtcNow;
        }
    }

    public class ItemCanceledEvent : INotification
    {
        public Guid SaleId { get; }
        public Guid ItemId { get; }
        public Guid ProductId { get; }
        public int Quantity { get; }
        public DateTime Timestamp { get; }

        public ItemCanceledEvent(Guid saleId, Guid itemId, Guid productId, int quantity)
        {
            SaleId = saleId;
            ItemId = itemId;
            ProductId = productId;
            Quantity = quantity;
            Timestamp = DateTime.UtcNow;
        }
    }
}