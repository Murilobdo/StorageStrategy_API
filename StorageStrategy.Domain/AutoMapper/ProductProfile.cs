using AutoMapper;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommand, ProductEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore())
                .ForMember(p => p.Category, opt => opt.Ignore());

            CreateMap<UpdateProductCommand, ProductEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore())
                .ForMember(p => p.Category, opt => opt.Ignore());

            CreateMap<DeleteProductCommand, ProductEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore())
                .ForMember(p => p.Category, opt => opt.Ignore());

            CreateMap<ProductEntity, CreateProductCommand>();
        }
    }
}
