using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Products;
using SalesManagement.Domain.Interfaces.Repositories;

namespace SalesManagement.Application.Queries.Handlers.Products
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedResult<ProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();

            // Aplicar Filtros
            if (!string.IsNullOrEmpty(request.Category))
                products = products.Where(p => p.Category.ToLower() == request.Category.ToLower());

            if (request.MinPrice.HasValue)
                products = products.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= request.MaxPrice.Value);

            if (request.IsActive.HasValue)
                products = products.Where(p => p.IsActive == request.IsActive.Value);

            // Aplicar classificação
            products = ApplySorting(products, request.SortBy, request.SortDirection);

            // Obter contagem total antes da paginação
            int totalItems = products.Count();

            // Aplicar paginação
            products = products.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            // Mapa para DTOs
            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products).ToList();

            return new PaginatedResult<ProductDTO>(productDtos, totalItems, request.Page, request.PageSize);
        }

        private IEnumerable<Domain.Entities.Product> ApplySorting(IEnumerable<Domain.Entities.Product> products, string sortBy, string sortDirection)
        {
            bool isAscending = string.IsNullOrEmpty(sortDirection) || sortDirection.ToLower() == "asc";

            switch (sortBy.ToLower())
            {
                case "name":
                    return isAscending ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name);
                case "price":
                    return isAscending ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price);
                case "category":
                    return isAscending ? products.OrderBy(p => p.Category) : products.OrderByDescending(p => p.Category);
                case "createdat":
                    return isAscending ? products.OrderBy(p => p.CreatedAt) : products.OrderByDescending(p => p.CreatedAt);
                default:
                    return isAscending ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name);
            }
        }
    }
}