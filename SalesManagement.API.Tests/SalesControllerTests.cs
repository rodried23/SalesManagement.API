using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SalesManagement.API.Controllers;
using SalesManagement.Application.Commands.Sales;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Sales;
using SalesManagement.Application.Services.Interfaces;
using SalesManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SalesManagement.API.Tests.Controllers
{
    public class SalesControllerTests
    {
        private readonly Mock<ISaleService> _saleServiceMock;
        private readonly SalesController _controller;

        public SalesControllerTests()
        {
            _saleServiceMock = new Mock<ISaleService>();
            _controller = new SalesController(_saleServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResultWithSales()
        {
            // Arrange
            var query = new GetAllSalesQuery { Page = 1, PageSize = 10 };

            var sales = new List<SaleDTO>
            {
                new SaleDTO {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    TotalAmount = 199.98m,
                    Status = (SaleStatus)0
                },
                new SaleDTO {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    TotalAmount = 299.97m,
                    Status = (SaleStatus)0
                }
            };

            var paginatedResult = new PaginatedResult<SaleDTO>(
                sales,        // data
                2,            // totalItems
                1,            // currentPage
                10            // pageSize
            );

            _saleServiceMock.Setup(service => service.GetAllSalesAsync(It.Is<GetAllSalesQuery>(q =>
                q.Page == query.Page && q.PageSize == query.PageSize)))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var returnValue = okResult.Value as PaginatedResult<SaleDTO>;
            returnValue.Should().NotBeNull();
            returnValue.Data.Should().HaveCount(2);
            returnValue.TotalItems.Should().Be(2);
            returnValue.CurrentPage.Should().Be(1);
            returnValue.PageSize.Should().Be(10);
            returnValue.TotalPages.Should().Be(1);

            _saleServiceMock.Verify(service => service.GetAllSalesAsync(It.IsAny<GetAllSalesQuery>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ExistingSale_ReturnsOkResult()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var saleDto = new SaleDTO
            {
                Id = saleId,
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                TotalAmount = 199.98m,
                Status = (SaleStatus)0,
                Items = new List<SaleItemDTO>
                {
                    new SaleItemDTO { ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 99.99m }
                }
            };

            _saleServiceMock.Setup(service => service.GetSaleByIdAsync(saleId))
                .ReturnsAsync(saleDto);

            // Act
            var result = await _controller.GetById(saleId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var returnValue = okResult.Value as SaleDTO;
            returnValue.Should().NotBeNull();
            returnValue.Id.Should().Be(saleId);

            _saleServiceMock.Verify(service => service.GetSaleByIdAsync(saleId), Times.Once);
        }

        [Fact]
        public async Task GetById_NonExistingSale_ReturnsNotFound()
        {
            // Arrange
            var saleId = Guid.NewGuid();

            _saleServiceMock.Setup(service => service.GetSaleByIdAsync(saleId))
                .ReturnsAsync((SaleDTO)null);

            // Act
            var result = await _controller.GetById(saleId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _saleServiceMock.Verify(service => service.GetSaleByIdAsync(saleId), Times.Once);
        }

        [Fact]
        public async Task Create_ValidSale_ReturnsCreatedAtAction()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<SaleItemCommand>
                {
                    new SaleItemCommand { ProductId = Guid.NewGuid(), Quantity = 2 }
                }
            };

            var newId = Guid.NewGuid();

            _saleServiceMock.Setup(service => service.CreateSaleAsync(It.IsAny<CreateSaleCommand>()))
                .ReturnsAsync(newId);

            // Act
            var result = await _controller.Create(command);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(SalesController.GetById));
            createdAtActionResult.RouteValues["id"].Should().Be(newId);
            createdAtActionResult.Value.Should().Be(newId);

            _saleServiceMock.Verify(service => service.CreateSaleAsync(It.IsAny<CreateSaleCommand>()), Times.Once);
        }

        [Fact]
        public async Task Update_ValidSale_ReturnsNoContent()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new UpdateSaleCommand
            {
                Id = saleId,
                // Removidas as propriedades que não existem na classe UpdateSaleCommand
                // Adicionando apenas a propriedade Id que é necessária
            };

            _saleServiceMock.Setup(service => service.UpdateSaleAsync(It.Is<UpdateSaleCommand>(c => c.Id == saleId)))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(command);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _saleServiceMock.Verify(service => service.UpdateSaleAsync(It.Is<UpdateSaleCommand>(c => c.Id == saleId)), Times.Once);
        }

        [Fact]
        public async Task Update_NonExistingSale_ReturnsNotFound()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new UpdateSaleCommand
            {
                Id = saleId,
                // Sem propriedades extras
            };

            _saleServiceMock.Setup(service => service.UpdateSaleAsync(It.Is<UpdateSaleCommand>(c => c.Id == saleId)))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(command);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _saleServiceMock.Verify(service => service.UpdateSaleAsync(It.Is<UpdateSaleCommand>(c => c.Id == saleId)), Times.Once);
        }

        [Fact]
        public async Task Cancel_ExistingSale_ReturnsNoContent()
        {
            // Arrange
            var saleId = Guid.NewGuid();

            _saleServiceMock.Setup(service => service.CancelSaleAsync(saleId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Cancel(saleId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _saleServiceMock.Verify(service => service.CancelSaleAsync(saleId), Times.Once);
        }

        [Fact]
        public async Task Cancel_NonExistingSale_ReturnsNotFound()
        {
            // Arrange
            var saleId = Guid.NewGuid();

            _saleServiceMock.Setup(service => service.CancelSaleAsync(saleId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Cancel(saleId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _saleServiceMock.Verify(service => service.CancelSaleAsync(saleId), Times.Once);
        }
    }
}