using AutoMapper;
using Checkout.PaymentsGateway.Contracts.V1.Requests;
using Checkout.PaymentsGateway.Domain.Models;

namespace Checkout.PaymentsGateway.Api.MappingProfiles
{
    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<CreatePaymentRequest, BankTransaction>();
        }
    }
}