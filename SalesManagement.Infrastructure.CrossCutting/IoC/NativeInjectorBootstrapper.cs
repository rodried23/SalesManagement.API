using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesManagement.Application.AutoMapper;
using SalesManagement.Application.Commands.Handlers.Products;
using SalesManagement.Application.Commands.Handlers.Sales;
using SalesManagement.Application.Commands.Products;
using SalesManagement.Application.Commands.Sales;
using SalesManagement.Application.Queries.Handlers.Products;
using SalesManagement.Application.Queries.Handlers.Sales;
using SalesManagement.Application.Queries.Products;
using SalesManagement.Application.Queries.Sales;
using SalesManagement.Application.Services.Implementations;
using SalesManagement.Application.Services.Interfaces;
using SalesManagement.Domain.Interfaces.Repositories;
using SalesManagement.Infrastructure.CrossCutting.Bus;
using SalesManagement.Infrastructure.CrossCutting.Identity.Services;
using SalesManagement.Infrastructure.CrossCutting.Logging;
using SalesManagement.Infrastructure.Data;
using SalesManagement.Infrastructure.Data.Context;
using SalesManagement.Infrastructure.Data.Repositories;

namespace SalesManagement.Infrastructure.CrossCutting.IoC
{
    public static class NativeInjectorBootstrapper
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ASP.NET Core
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateSaleCommand).Assembly));

            // Application - Services
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IProductService, ProductService>();

            // Application - Commands
            services.AddScoped<IRequestHandler<CreateSaleCommand, Guid>, CreateSaleCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateSaleCommand, bool>, UpdateSaleCommandHandler>();
            services.AddScoped<IRequestHandler<CancelSaleCommand, bool>, CancelSaleCommandHandler>();
            services.AddScoped<IRequestHandler<CreateProductCommand, Guid>, CreateProductCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateProductCommand, bool>, UpdateProductCommandHandler>();

            // Application - Queries
            services.AddScoped<IRequestHandler<GetSaleQuery, Application.DTOs.SaleDTO>, GetSaleQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllSalesQuery, Application.DTOs.PaginatedResult<Application.DTOs.SaleDTO>>, GetAllSalesQueryHandler>();
            services.AddScoped<IRequestHandler<GetProductQuery, Application.DTOs.ProductDTO>, GetProductQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllProductsQuery, Application.DTOs.PaginatedResult<Application.DTOs.ProductDTO>>, GetAllProductsQueryHandler>();

            // Infra - Data
            services.AddDbContext<SalesDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Infra - Data - Repositories
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infra - CrossCutting
            services.AddScoped<IBus, InMemoryBus>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<MongoContext>();
        }
    }
}