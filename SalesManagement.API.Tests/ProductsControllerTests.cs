using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SalesManagement.API.Controllers;
using SalesManagement.Application.Commands.Products;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Products;
using SalesManagement.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SalesManagement.API.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductsController(_productServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResultWithProducts()
        {
            // Arrange
            var query = new GetAllProductsQuery { Page = 1, PageSize = 10 };

            var products = new List<ProductDTO>
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

            var paginatedResult = new PaginatedResult<ProductDTO>(
                products,      // data
                2,             // totalItems
                1,             // currentPage
                10             // pageSize
            );

            _productServiceMock.Setup(service => service.GetAllProductsAsync(It.Is<GetAllProductsQuery>(q =>
                q.Page == query.Page && q.PageSize == query.PageSize)))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var returnValue = okResult.Value as PaginatedResult<ProductDTO>;
            returnValue.Should().NotBeNull();
            returnValue.Data.Should().HaveCount(2);
            returnValue.TotalItems.Should().Be(2);
            returnValue.CurrentPage.Should().Be(1);
            returnValue.PageSize.Should().Be(10);

            _productServiceMock.Verify(service => service.GetAllProductsAsync(It.IsAny<GetAllProductsQuery>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ExistingProduct_ReturnsOkResult()
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
                CreatedAt = DateTime.UtcNow
            };

            _productServiceMock.Setup(service => service.GetProductByIdAsync(productId))
                .ReturnsAsync(productDto);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            // Corrigido: IActionResult não tem Result
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var returnValue = okResult.Value as ProductDTO;
            returnValue.Should().NotBeNull();
            returnValue.Id.Should().Be(productId);

            _productServiceMock.Verify(service => service.GetProductByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetById_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productServiceMock.Setup(service => service.GetProductByIdAsync(productId))
                .ReturnsAsync((ProductDTO)null);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            // Corrigido: IActionResult não tem Result
            result.Should().BeOfType<NotFoundResult>();
            _productServiceMock.Verify(service => service.GetProductByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task Create_ValidProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "New Product",
                Description = "New Description",
                Price = 129.99m,
                Category = "Office",
                ImageUrl = "http://example.com/image.jpg"
            };

            var newId = Guid.NewGuid();

            _productServiceMock.Setup(service => service.CreateProductAsync(It.IsAny<CreateProductCommand>()))
                .ReturnsAsync(newId);

            // Act
            var result = await _controller.Create(command);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(ProductsController.GetById));
            createdAtActionResult.RouteValues["id"].Should().Be(newId);
            createdAtActionResult.Value.Should().Be(newId);

            _productServiceMock.Verify(service => service.CreateProductAsync(It.IsAny<CreateProductCommand>()), Times.Once);
        }

        [Fact]
        public async Task Update_ValidProduct_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new UpdateProductCommand
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 149.99m,
                Category = "Office",
                ImageUrl = "http://example.com/updated.jpg"
            };

            _productServiceMock.Setup(service => service.UpdateProductAsync(It.Is<UpdateProductCommand>(c => c.Id == productId)))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(command);

            // Assert
            result.Should().BeOfType<OkResult>();
            _productServiceMock.Verify(service => service.UpdateProductAsync(It.Is<UpdateProductCommand>(c => c.Id == productId)), Times.Once);
        }

        [Fact]
        public async Task Update_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new UpdateProductCommand
            {
                Id = productId,
                Name = "Updated Product"
            };

            _productServiceMock.Setup(service => service.UpdateProductAsync(It.Is<UpdateProductCommand>(c => c.Id == productId)))
                .ReturnsAsync(false);

            // Act
            // Corrigido: chamando com apenas um argumento
            var result = await _controller.Update(command);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _productServiceMock.Verify(service => service.UpdateProductAsync(It.Is<UpdateProductCommand>(c => c.Id == productId)), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingProduct_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productServiceMock.Setup(service => service.DeleteProductAsync(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            result.Should().BeOfType<OkResult>();
            _productServiceMock.Verify(service => service.DeleteProductAsync(productId), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productServiceMock.Setup(service => service.DeleteProductAsync(productId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _productServiceMock.Verify(service => service.DeleteProductAsync(productId), Times.Once);
        }
    }
}