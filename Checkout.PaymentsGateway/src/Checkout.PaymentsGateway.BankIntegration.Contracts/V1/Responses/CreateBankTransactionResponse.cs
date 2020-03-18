namespace Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Responses
{
    public class CreateBankTransactionResponse
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }
}