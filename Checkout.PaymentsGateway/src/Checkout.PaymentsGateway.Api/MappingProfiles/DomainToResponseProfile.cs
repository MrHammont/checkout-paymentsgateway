using AutoMapper;
using Checkout.PaymentsGateway.Contracts.V1.Responses;
using Checkout.PaymentsGateway.Domain.Models;

namespace Checkout.PaymentsGateway.Api.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<PaymentRecord, GetPaymentResponse>()
                .ForMember(
                    dest => dest.TransactionId,
                    opt =>
                        opt.MapFrom(from => from.Id))
                .ForMember(
                    dest => dest.CardDetails,
                    opt =>
                        opt.MapFrom(from => new CardDetails
                        {
                            HolderName = from.CardName,
                            ExpirationDate = $"{from.CardExpirationMonth}/{from.CardExpirationYear}",
                            Number = from.CardNumber
                        }))
                .ForMember(dest => dest.TransactionDetails,
                    opt =>
                        opt.MapFrom(from => new TransactionDetails()
                        {
                            Amount = from.Amount,
                            Currency = from.Currency,
                            TransactionDate = from.TransactionDate,
                            TransactionStatus = from.TransactionStatus
                        }));

            CreateMap<CreatePaymentResponse, BankTransaction>();
        }
    }
}