using FluentAssertions;
using SalesManagement.Domain.Entities;
using SalesManagement.Domain.Exceptions;
using System;
using Xunit;

namespace SalesManagement.Domain.Tests
{
    public class ProductTests
    {
        [Fact]
        public void CreateProduct_WithValidParameters_ShouldCreateProductSuccessfully()
        {
            // Arrange
            var name = "Test Product";
            var description = "Test Description";
            var category = "Electronics";
            var price = 99.99m;
            var imageUrl = "http://example.com/image.jpg";

            // Act
            var product = new Product(name, description, category, price, imageUrl);

            // Assert
            product.Should().NotBeNull();
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.Category.Should().Be(category);
            product.Price.Should().Be(price);
            product.ImageUrl.Should().Be(imageUrl);
            product.IsActive.Should().BeTrue(); // Verificando o valor padrão IsActive
        }

        [Fact]
        public void CreateProduct_WithoutImageUrl_ShouldCreateProductSuccessfully()
        {
            // Arrange
            var name = "Test Product";
            var description = "Test Description";
            var category = "Electronics";
            var price = 99.99m;

            // Act - sem passar o parâmetro opcional imageUrl
            var product = new Product(name, description, category, price);

            // Assert
            product.Should().NotBeNull();
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.Category.Should().Be(category);
            product.Price.Should().Be(price);
            product.ImageUrl.Should().BeNull();
            product.IsActive.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "Description", "Category", 99.99)]
        [InlineData("Name", "", "Category", 99.99)]
        [InlineData("Name", "Description", "", 99.99)]
        [InlineData("Name", "Description", "Category", -1)]
        public void CreateProduct_WithInvalidParameters_ShouldThrowDomainException(
            string name, string description, string category,
            double price)
        {
            // Act & Assert
            Action act = () => new Product(name, description, category, Convert.ToDecimal(price));
            // Nota: sua classe lança DomainException, não ArgumentException
            act.Should().Throw<DomainException>()
                .WithMessage("O preço deve ser maior que zero");
        }

        // Se sua classe Product tiver métodos adicionais que você queira testar,
        // você pode adicionar testes para eles aqui
    }
}