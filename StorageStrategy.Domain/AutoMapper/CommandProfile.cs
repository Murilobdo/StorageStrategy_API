using AutoMapper;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<CreateCommandCommand, CommandEntity>()
                .ForMember(p => p.Company, opt => opt.Ignore())
                .ForMember(p => p.Items, opt => opt.Ignore())
                .ForMember(p => p.Employee, opt => opt.Ignore());

            CreateMap<CommandEntity, CreateCommandCommand>()
                .ForMember(p => p.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<UpdateCommandCommand, CommandEntity>()
                .ForMember(p => p.Items, opt => opt.MapFrom(src =>src.Items))
                .ForMember(p => p.Company, opt => opt.Ignore())
                .ForMember(p => p.Employee, opt => opt.Ignore());

            CreateMap<DeleteCommandCommand, CommandEntity>()
                .ForMember(p => p.Items, opt => opt.MapFrom(src =>src.Items))
                .ForMember(p => p.Company, opt => opt.Ignore())
                .ForMember(p => p.Employee, opt => opt.Ignore());

            CreateMap<CommandItemBase, CommandItem>();
            CreateMap<CommandItem, CommandItemBase>();

        }
    }
}
