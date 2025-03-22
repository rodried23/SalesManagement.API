using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SalesManagement.Application.DTOs;
using SalesManagement.Application.Queries.Products;
using SalesManagement.Domain.Interfaces.Repositories;

namespace SalesManagement.Application.Queries.Handlers.Products
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
            return _mapper.Map<ProductDTO>(product);
        }
    }
}