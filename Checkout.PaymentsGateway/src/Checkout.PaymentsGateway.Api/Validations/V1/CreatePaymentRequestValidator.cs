using System;
using Checkout.PaymentsGateway.Contracts.V1.Requests;
using FluentValidation;

namespace Checkout.PaymentsGateway.Api.Validations.V1
{
    public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
    {
        public CreatePaymentRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("Credit card number is required")
                .CreditCard()
                .WithMessage("Credit card number is not valid");

            RuleFor(x => x.CardExpirationMonth)
                .NotEmpty()
                .NotNull()
                .WithMessage("Credit card expiry month is required")
                .Length(2)
                .Must(BeValidMonth)
                .WithMessage("The credit card expiry month is invalid")
                .Must(BeMonthGreaterOrEqualsThanCurrent)
                .When(ExpirationYearIsCurrent, ApplyConditionTo.CurrentValidator)
                .WithMessage("The credit card expiry month is invalid");

            RuleFor(x => x.CardExpirationYear)
                .NotEmpty()
                .NotNull()
                .WithMessage("Credit card expiry year is required")
                .Length(4)
                .Must(BeYearGreaterOrEqualsThanCurrent)
                .WithMessage("The credit card expiry year is invalid");

            RuleFor(x => x.CardHolderName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Card holder name is required");

            RuleFor(x => x.Amount)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("The amount is not valid");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .NotNull()
                .WithMessage("Currency is required")
                .IsEnumName(typeof(Currency), false)
                .WithMessage($"Valid currencies are only: {GetValidCurrencies()}");

            RuleFor(x => x.Cvv)
                .NotNull()
                .NotEmpty()
                .Length(3)
                .Must(BeValidCvv)
                .WithMessage("CVV is not valid");
        }

        private static string GetValidCurrencies()
        {
            return string.Join(" - ", Enum.GetNames(typeof(Currency)));
        }

        private static bool BeValidCvv(string x)
        {
            return int.TryParse(x, out var cvv) && cvv > 0;
        }

        private static bool BeYearGreaterOrEqualsThanCurrent(string x)
        {
            return int.TryParse(x, out var year) && year >= DateTime.Now.Year;
        }

        private static bool ExpirationYearIsCurrent(CreatePaymentRequest x)
        {
            return int.TryParse(x.CardExpirationYear, out var year) && year == DateTime.Now.Year;
        }

        private static bool BeMonthGreaterOrEqualsThanCurrent(string x)
        {
            return int.TryParse(x, out var month) && month >= DateTime.Now.Month;
        }

        private static bool BeValidMonth(string x)
        {
            return int.TryParse(x, out var month) && month >= 0 && month <= 12;
        }
    }
}