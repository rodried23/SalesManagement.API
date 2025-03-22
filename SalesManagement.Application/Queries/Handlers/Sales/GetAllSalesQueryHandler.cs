using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Sales;
using SalesManagement.Domain.Interfaces.Repositories;

namespace SalesManagement.Application.Queries.Handlers.Sales
{
    public class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, PaginatedResult<SaleDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSalesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<SaleDTO>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var sales = await _unitOfWork.SaleRepository.GetAllAsync();

            // Aplicar Filtros
            if (request.CustomerId.HasValue)
                sales = sales.Where(s => s.CustomerId == request.CustomerId.Value);

            if (request.BranchId.HasValue)
                sales = sales.Where(s => s.BranchId == request.BranchId.Value);

            if (request.StartDate.HasValue)
                sales = sales.Where(s => s.SaleDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                sales = sales.Where(s => s.SaleDate <= request.EndDate.Value);

            // Aplicar classificação
            sales = ApplySorting(sales, request.SortBy, request.SortDirection);

            // Obter contagem total antes da paginação
            int totalItems = sales.Count();

            // Aplicar paginação
            sales = sales.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            // Mapa para DTOs
            var salesDtos = _mapper.Map<IEnumerable<SaleDTO>>(sales).ToList();

            return new PaginatedResult<SaleDTO>(salesDtos, totalItems, request.Page, request.PageSize);
        }

        private IEnumerable<Domain.Entities.Sale> ApplySorting(IEnumerable<Domain.Entities.Sale> sales, string sortBy, string sortDirection)
        {
            bool isAscending = string.IsNullOrEmpty(sortDirection) || sortDirection.ToLower() == "asc";

            switch (sortBy.ToLower())
            {
                case "salenumber":
                    return isAscending ? sales.OrderBy(s => s.SaleNumber) : sales.OrderByDescending(s => s.SaleNumber);
                case "saledate":
                    return isAscending ? sales.OrderBy(s => s.SaleDate) : sales.OrderByDescending(s => s.SaleDate);
                case "customername":
                    return isAscending ? sales.OrderBy(s => s.CustomerName) : sales.OrderByDescending(s => s.CustomerName);
                case "branchname":
                    return isAscending ? sales.OrderBy(s => s.BranchName) : sales.OrderByDescending(s => s.BranchName);
                case "totalamount":
                    return isAscending ? sales.OrderBy(s => s.TotalAmount) : sales.OrderByDescending(s => s.TotalAmount);
                default:
                    return isAscending ? sales.OrderBy(s => s.SaleDate) : sales.OrderByDescending(s => s.SaleDate);
            }
        }
    }
}