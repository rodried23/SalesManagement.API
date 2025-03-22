using System;
using SalesManagement.Domain.Events;
using SalesManagement.Domain.Exceptions;

namespace SalesManagement.Domain.Entities
{
    public class SaleItem : EntityBase
    {
        public Guid SaleId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalPrice => (UnitPrice * Quantity) - Discount;


        // Propriedade de navegação
        public virtual Sale Sale { get; private set; }

        protected SaleItem() { }

        public SaleItem(Guid saleId, Guid productId, string productName, int quantity, decimal unitPrice)
        {

            // Regra de validação: Não é possível vender mais de 20 itens idênticos
            if (quantity > 20)
                throw new DomainException(@"Não é possível vender mais de 20 itens idênticos");

            if (quantity <= 0)
                throw new DomainException(@"Quantidade deve ser maior que zero");

            if (unitPrice <= 0)
                throw new DomainException(@"Preço unitário deve ser maior que zero");

            SaleId = saleId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;

            CalculateDiscount();
        }

        public void UpdateQuantity(int quantity)
        {

            // Regra de validação: Não é possível vender mais de 20 itens idênticos
            if (quantity > 20)
                throw new DomainException(@"Não é possível vender mais de 20 itens idênticos");

            if (quantity <= 0)
                throw new DomainException(@"A quantidade deve ser maior que zero");

            Quantity = quantity;
            CalculateDiscount();
            UpdatedAtNow();
        }

        public void Cancel()
        {
            AddDomainEvent(new ItemCanceledEvent(SaleId, Id, ProductId, Quantity));
        }

        private void CalculateDiscount()
        {
            decimal discountPercentage = 0;

            // Regras de Negócios:
            // - Compras com mais de 4 itens idênticos ganham 10% de desconto
            // - Compras entre 10-20 itens idênticos ganham 20% de desconto
            // - Compras abaixo de 4 itens não podem ter descontos

            if (Quantity >= 10 && Quantity <= 20)
            {
                discountPercentage = 0.2m; // 20% Desconto
            }
            else if (Quantity >= 4)
            {
                discountPercentage = 0.1m; // 10% Desconto
            }

            Discount = UnitPrice * Quantity * discountPercentage;
        }
    }
}