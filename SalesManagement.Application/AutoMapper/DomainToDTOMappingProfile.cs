using AutoMapper;
using SalesManagement.Application.DTOs;
using SalesManagement.Domain.Entities;

namespace SalesManagement.Application.AutoMapper
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<Branch, BranchDTO>();

            CreateMap<SaleItem, SaleItemDTO>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));

            CreateMap<Sale, SaleDTO>()
                .ForMember(dest => dest.IsCanceled, opt => opt.MapFrom(src => src.IsCanceled));
        }
    }
}