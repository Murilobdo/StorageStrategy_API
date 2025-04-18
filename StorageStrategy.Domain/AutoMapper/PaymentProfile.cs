using AutoMapper;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<PaymentCommand, PaymentEntity>();

        CreateMap<PaymentEntity, PaymentCommand>();
    }
}