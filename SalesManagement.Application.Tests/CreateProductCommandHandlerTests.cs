using FluentAssertions;
using SalesManagement.Domain.Entities;
using SalesManagement.Domain.Enums;
using SalesManagement.Domain.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace SalesManagement.Domain.Tests
{
    public class SaleTests
    {
        [Fact]
        public void CreateSale_WithValidParameters_ShouldCreateSaleSuccessfully()
        {
            // Arrange
            var saleNumber = 12345;
            var customerId = Guid.NewGuid();
            var customerName = "Cliente Teste";
            var branchId = Guid.NewGuid();
            var branchName = "Filial Teste";

            // Act
            var sale = new Sale(saleNumber, customerId, customerName, branchId, branchName);

            // Assert
            sale.Should().NotBeNull();
            sale.SaleNumber.Should().Be(saleNumber);
            sale.CustomerId.Should().Be(customerId);
            sale.CustomerName.Should().Be(customerName);
            sale.BranchId.Should().Be(branchId);
            sale.BranchName.Should().Be(branchName);
            sale.Status.Should().Be(SaleStatus.Created);
            sale.SaleDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            sale.TotalAmount.Should().Be(0);
            sale.Items.Should().BeEmpty();
            sale.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public void AddItem_WithValidParameters_ShouldAddItemAndUpdateTotal()
        {
            // Arrange
            var sale = CreateValidSale();
            var productId = Guid.NewGuid();
            var productName = "Produto Teste";
            var quantity = 2;
            var unitPrice = 50m;

            // Act
            sale.AddItem(productId, productName, quantity, unitPrice);

            // Assert
            sale.Items.Should().HaveCount(1);
            var addedItem = sale.Items.First();
            addedItem.ProductId.Should().Be(productId);
            addedItem.ProductName.Should().Be(productName);
            addedItem.Quantity.Should().Be(quantity);
            addedItem.UnitPrice.Should().Be(unitPrice);
            sale.TotalAmount.Should().Be(100m); // 2 * 50
        }

        [Fact]
        public void AddMultipleItems_ShouldCalculateTotalCorrectly()
        {
            // Arrange
            var sale = CreateValidSale();

            // Act
            sale.AddItem(Guid.NewGuid(), "Produto 1", 2, 50m);
            sale.AddItem(Guid.NewGuid(), "Produto 2", 3, 30m);

            // Assert
            sale.Items.Should().HaveCount(2);
            sale.TotalAmount.Should().Be(190m); // (2 * 50) + (3 * 30)
        }

        [Fact]
        public void AddItem_WithExcessiveQuantity_ShouldThrowDomainException()
        {
            // Arrange
            var sale = CreateValidSale();
            var productId = Guid.NewGuid();

            // Act & Assert
            Action act = () => sale.AddItem(productId, "Produto Teste", 21, 50m);
            act.Should().Throw<DomainException>()
                .WithMessage("Não é possível vender mais de 20 itens idênticos");
        }

        [Fact]
        public void UpdateItem_WithValidParameters_ShouldUpdateQuantityAndRecalculateTotal()
        {
            // Arrange
            var sale = CreateValidSale();
            sale.AddItem(Guid.NewGuid(), "Produto Teste", 2, 50m);
            var itemId = sale.Items.First().Id;

            // Act
            sale.UpdateItem(itemId, 3);

            // Assert
            sale.Items.First().Quantity.Should().Be(3);
            sale.TotalAmount.Should().Be(150m); // 3 * 50
        }

        [Fact]
        public void UpdateItem_WithNonexistentItemId_ShouldThrowDomainException()
        {
            // Arrange
            var sale = CreateValidSale();
            var nonExistentItemId = Guid.NewGuid();

            // Act & Assert
            Action act = () => sale.UpdateItem(nonExistentItemId, 3);
            act.Should().Throw<DomainException>()
                .WithMessage("Item não encontrado nesta venda");
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItemAndRecalculateTotal()
        {
            // Arrange
            var sale = CreateValidSale();
            sale.AddItem(Guid.NewGuid(), "Produto 1", 2, 50m);
            sale.AddItem(Guid.NewGuid(), "Produto 2", 3, 30m);
            var itemIdToRemove = sale.Items.First().Id;

            // Act
            sale.RemoveItem(itemIdToRemove);

            // Assert
            sale.Items.Should().HaveCount(1);
            sale.TotalAmount.Should().Be(90m); // 3 * 30
        }

        [Fact]
        public void RemoveItem_WithNonexistentItemId_ShouldThrowDomainException()
        {
            // Arrange
            var sale = CreateValidSale();
            var nonExistentItemId = Guid.NewGuid();

            // Act & Assert
            Action act = () => sale.RemoveItem(nonExistentItemId);
            act.Should().Throw<DomainException>()
                .WithMessage("Item não encontrado nesta venda");
        }

        [Fact]
        public void Cancel_ShouldSetStatusToCanceled()
        {
            // Arrange
            var sale = CreateValidSale();

            // Act
            sale.Cancel();

            // Assert
            sale.Status.Should().Be(SaleStatus.Canceled);
            sale.IsCanceled.Should().BeTrue();
        }

        [Fact]
        public void Cancel_AlreadyCanceledSale_ShouldThrowDomainException()
        {
            // Arrange
            var sale = CreateValidSale();
            sale.Cancel(); // Cancela uma vez

            // Act & Assert
            Action act = () => sale.Cancel(); // Tenta cancelar novamente
            act.Should().Throw<DomainException>()
                .WithMessage("A venda já foi cancelada");
        }

        // Método auxiliar para criar uma venda válida para testes
        private Sale CreateValidSale()
        {
            return new Sale(
                12345,
                Guid.NewGuid(),
                "Cliente Teste",
                Guid.NewGuid(),
                "Filial Teste"
            );
        }
    }
}