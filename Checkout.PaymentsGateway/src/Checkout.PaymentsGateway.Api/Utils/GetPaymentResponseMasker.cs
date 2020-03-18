using Checkout.PaymentsGateway.Domain.Models;

namespace Checkout.PaymentsGateway.Api.Utils
{
    public class GetPaymentResponseMasker : IGetPaymentResponseMasker
    {
        public PaymentRecord MaskPaymentRecord(PaymentRecord paymentRecord)
        {
            paymentRecord.CardNumber = MaskCreditCard(paymentRecord.CardNumber);

            return paymentRecord;
        }

        private string MaskCreditCard(string creditCard)
        {
            if (string.IsNullOrEmpty(creditCard))
                return creditCard;

            var numberOfVisibleNumbers = GetNumberVisibleNumbers(creditCard.Length);

            var stringArray = creditCard.ToCharArray();

            for (var i = stringArray.Length - 1 - numberOfVisibleNumbers; i >= 0; i--) stringArray[i] = '*';

            return string.Join("", stringArray);
        }

        // Size guard to avoid out of range
        // Credit cards should never be this small and validation happens on request received
        // Exceptions or alerts set depending on business rules
        private int GetNumberVisibleNumbers(int creditCardLength)
        {
            var size = 4;

            if (creditCardLength <= size)
                size = creditCardLength;

            return size;
        }
    }
}