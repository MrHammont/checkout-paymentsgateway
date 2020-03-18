using AutoMapper;
using Checkout.PaymentsGateway.DataContext.Models;
using Checkout.PaymentsGateway.Domain.Models;

namespace Checkout.PaymentsGateway.Api.MappingProfiles
{
    public class InfraToDomain : Profile
    {
        public InfraToDomain()
        {
            CreateMap<Payment, PaymentRecord>()
                .ForMember(
                    dest => dest.CardExpirationYear,
                    opt => opt.MapFrom(src => src.CardExpirationDate.Year.ToString()))
                .ForMember(
                    dest => dest.CardExpirationMonth,
                    opt => opt.MapFrom(src => src.CardExpirationDate.Month.ToString("d2")));
        }
    }
}