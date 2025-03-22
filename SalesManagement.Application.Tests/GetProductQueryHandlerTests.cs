using AutoMapper;
using FluentAssertions;
using Moq;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Handlers.Products;
using SalesManagement.Application.Queries.Products;
using SalesManagement.Domain.Entities;
using SalesManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SalesManagement.Application.Tests.Queries.Handlers
{
    public class GetProductQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetProductQueryHandler _handler;

        public GetProductQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();

            // Configurando o UnitOfWork para retornar o repositório mockado
            _unitOfWorkMock.Setup(uow => uow.ProductRepository).Returns(_productRepositoryMock.Object);

            _handler = new GetProductQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingProduct_ShouldReturnProductDTO()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Criando a query - assumindo que GetProductQuery tem uma propriedade Id
            var query = new GetProductQuery { Id = productId };

            // Criando um produto - ajustando os parâmetros do construtor
            // A assinatura exata pode variar - ajuste conforme necessário
            var product = new Product(
                "Test Product",       // name
                "Test Description",   // description
                "Electronics",        // category
                99.99m                // price
            );

            // Definindo o Id do produto para testes
            SetEntityId(product, productId);

            // Criando o DTO que deve ser retornado pelo mapper
            var productDto = new ProductDTO
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                Category = "Electronics"
                // Adicione outras propriedades que o DTO tenha
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _mapperMock.Setup(mapper => mapper.Map<ProductDTO>(product))
                .Returns(productDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            result.Name.Should().Be("Test Product");
            result.Description.Should().Be("Test Description");
            result.Price.Should().Be(99.99m);
            result.Category.Should().Be("Electronics");

            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ProductDTO>(product), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingProduct_ShouldReturnNull()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Criando a query
            var query = new GetProductQuery { Id = productId };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            _mapperMock.Setup(mapper => mapper.Map<ProductDTO>(null))
                .Returns((ProductDTO)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ProductDTO>(null), Times.Once);
        }

        // Método auxiliar para definir o Id da entidade para testes
        private void SetEntityId(EntityBase entity, Guid id)
        {
            // Usando reflexão para definir o Id
            var property = typeof(EntityBase).GetProperty("Id");

            // Tentativa 1: Obter o campo de backing
            var backingField = entity.GetType().GetField($"<{property.Name}>k__BackingField",
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
            var idField = entity.GetType().GetField("_id",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (idField != null)
            {
                idField.SetValue(entity, id);
            }
        }
    }
}