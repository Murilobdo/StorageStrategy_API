using AutoMapper;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryCommand, CategoryEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore());

            CreateMap<CategoryEntity, CreateCategoryCommand>();

            CreateMap<UpdateCategoryCommand, CategoryEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore());

            CreateMap<DeleteCategoryCommand, CategoryEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore());

        }
    }
}
