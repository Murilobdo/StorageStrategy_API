using AutoMapper;
using StorageStrategy.Domain.Commands.PaymentMethod;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.AutoMapper;

public class PaymentMethodProfile : Profile
{
    public PaymentMethodProfile()
    {
        
        CreateMap<PaymentMethodEntity, CreatePaymentMethodCommand>();

        CreateMap<CreatePaymentMethodCommand, PaymentMethodEntity>();
        CreateMap<UpdatePaymentMethodCommand, PaymentMethodEntity>();

    }
}