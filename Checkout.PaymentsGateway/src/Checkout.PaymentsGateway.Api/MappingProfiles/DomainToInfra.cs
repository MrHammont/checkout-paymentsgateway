using AutoMapper;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Requests;
using Checkout.PaymentsGateway.DataContext.Models;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Infrastructure.Extensions;

namespace Checkout.PaymentsGateway.Api.MappingProfiles
{
    public class DomainToInfra : Profile
    {
        public DomainToInfra()
        {
            CreateMap<BankTransaction, CreateBankTransactionRequest>();
            CreateMap<PaymentRecord, Payment>()
                .ForMember(
                    dest => dest.CardExpirationDate,
                    opt => opt.MapFrom(src => src.GetDateTimeFromCardDetails()));
        }
    }
}