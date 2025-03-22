using System;
using System.Collections.Generic;
using System.Linq;
using SalesManagement.Domain.Enums;
using SalesManagement.Domain.Events;
using SalesManagement.Domain.Exceptions;

namespace SalesManagement.Domain.Entities
{
    public class Sale : EntityBase
    {
        public int SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; }
        public decimal TotalAmount { get; private set; }
        public SaleStatus Status { get; private set; }
        public bool IsCanceled => Status == SaleStatus.Canceled;

        private readonly List<SaleItem> _items;
        public virtual IReadOnlyCollection<SaleItem> Items => _items;

        protected Sale()
        {
            _items = new List<SaleItem>();
        }

        public Sale(int saleNumber, Guid customerId, string customerName, Guid branchId, string branchName)
            : this()
        {
            SaleNumber = saleNumber;
            SaleDate = DateTime.UtcNow;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            Status = SaleStatus.Created;

            // Adicionar evento de domínio para criação de venda
            AddDomainEvent(new SaleCreatedEvent(Id, SaleNumber, CustomerId, BranchId));
        }

        public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            // Validation rule: Cannot sell more than 20 identical items
            if (quantity > 20)
                throw new DomainException(@"Não é possível vender mais de 20 itens idênticos");

            var item = new SaleItem(Id, productId, productName, quantity, unitPrice);
            _items.Add(item);

            RecalculateTotalAmount();

            AddDomainEvent(new SaleModifiedEvent(Id, SaleNumber));
            UpdatedAtNow();
        }

        public void UpdateItem(Guid itemId, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                throw new DomainException(@"Item não encontrado nesta venda");

            item.UpdateQuantity(quantity);

            RecalculateTotalAmount();

            AddDomainEvent(new SaleModifiedEvent(Id, SaleNumber));
            UpdatedAtNow();
        }

        public void RemoveItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                throw new DomainException(@"Item não encontrado nesta venda");

            item.Cancel();
            _items.Remove(item);

            RecalculateTotalAmount();

            AddDomainEvent(new SaleModifiedEvent(Id, SaleNumber));
            UpdatedAtNow();
        }

        public void Cancel()
        {
            if (Status == SaleStatus.Canceled)
                throw new DomainException(@"A venda já foi cancelada");

            Status = SaleStatus.Canceled;

            AddDomainEvent(new SaleCanceledEvent(Id, SaleNumber));
            UpdatedAtNow();
        }

        private void RecalculateTotalAmount()
        {
            TotalAmount = _items.Sum(i => i.TotalPrice);
        }
    }
}