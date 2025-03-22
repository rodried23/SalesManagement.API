using FluentAssertions;
using MediatR;
using Moq;
using SalesManagement.Application.Commands.Products;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Products;
using SalesManagement.Application.Services.Implementations;
using SalesManagement.Domain.Entities;
using SalesManagement.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SalesManagement.Application.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IProductRepository>();

            _unitOfWorkMock.Setup(uow => uow.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _productService = new ProductService(_mediatorMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetProductByIdAsync_ExistingProduct_ShouldReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new ProductDTO
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                Category = "Electronics",
                ImageUrl = "http://example.com/image.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _mediatorMock.Setup(m => m.Send(It.Is<GetProductQuery>(q => q.Id == productId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDto);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            result.Name.Should().Be("Test Product");
            result.Description.Should().Be("Test Description");
            result.Price.Should().Be(99.99m);
            result.Category.Should().Be("Electronics");
            result.ImageUrl.Should().Be("http://example.com/image.jpg");
            result.IsActive.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(It.Is<GetProductQuery>(q => q.Id == productId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnPaginatedProducts()
        {
            // Arrange
            var query = new GetAllProductsQuery { Page = 1, PageSize = 10 };

            // Criando a lista de produtos
            var productsList = new List<ProductDTO>
    {
        new ProductDTO
        {
            Id = Guid.NewGuid(),
            Name = "Product 1",
            Price = 99.99m,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new ProductDTO
        {
            Id = Guid.NewGuid(),
            Name = "Product 2",
            Price = 149.99m,
            Category = "Office",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        }
    };

            // Criando o objeto PaginatedResult com o construtor correto
            var paginatedResult = new PaginatedResult<ProductDTO>(
                productsList,    // data
                2,               // totalItems
                1,               // currentPage
                10               // pageSize
            );

            _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _productService.GetAllProductsAsync(query);

            // Assert
            result.Should().NotBeNull();

            // Usando as propriedades corretas da classe PaginatedResult
            result.Data.Should().HaveCount(2);
            result.TotalItems.Should().Be(2);
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalPages.Should().Be(1); // (2 + 10 - 1) / 10 = 1

            _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_ValidCommand_ShouldReturnNewId()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "New Product",
                Description = "New Description",
                Price = 129.99m,
                Category = "Office",
                ImageUrl = "http://example.com/new.jpg"
            };

            var newId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newId);

            // Act
            var result = await _productService.CreateProductAsync(command);

            // Assert
            result.Should().Be(newId);
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ValidCommand_ShouldReturnSuccess()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 149.99m,
                Category = "Electronics",
                ImageUrl = "http://example.com/updated.jpg"
            };

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _productService.UpdateProductAsync(command);

            // Assert
            result.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ExistingProduct_ShouldDeactivateAndReturnTrue()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Test Product", "Test Description", "Electronics", 99.99m);

            // Usando reflexão para definir o Id
            SetEntityId(product, productId);

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(product))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _productService.DeleteProductAsync(productId);

            // Assert
            result.Should().BeTrue();
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(product), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_NonExistingProduct_ShouldReturnFalse()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.DeleteProductAsync(productId);

            // Assert
            result.Should().BeFalse();
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }

        // Método auxiliar para definir o Id da entidade para testes
        private void SetEntityId(EntityBase entity, Guid id)
        {
            // Usando reflexão para definir o Id
            var property = typeof(EntityBase).GetProperty("Id");

            // Tentativa 1: Obter o campo de backing
            var backingField = typeof(EntityBase).GetField($"<{property.Name}>k__BackingField",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (backingField != null)
            {
                backingField.SetValue(entity, id);
                return;
            }

            // Tentativa 2: Usar o setter privado
            var setter = property.GetSetMethod(true); // true para obter o setter privado
            if (setter != null)
            {
                setter.Invoke(entity, new object[] { id });
                return;
            }

            // Tentativa 3: Campo _id diretamente
            var idField = typeof(EntityBase).GetField("_id",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (idField != null)
            {
                idField.SetValue(entity, id);
            }
        }
    }
}