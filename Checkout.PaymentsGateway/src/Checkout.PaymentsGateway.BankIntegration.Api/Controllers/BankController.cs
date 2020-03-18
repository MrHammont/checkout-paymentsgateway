using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Requests;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentsGateway.BankIntegration.Api.Controllers
{
    [ApiController]
    public class BankController : ControllerBase
    {
        [HttpPost(ApiRoutes.Bank.Post)]
        public async Task<IActionResult> Post([FromBody] CreateBankTransactionRequest request, [FromQuery] string status = null)
        {
            var transactionId = Guid.NewGuid().ToString();
            var response = new CreateBankTransactionResponse()
            {
                Status = status ?? GetRandomStatus(),
                TransactionId = transactionId
            };

            return Ok(response);
        }

        private string GetRandomStatus()
        {
            var random = new Random();
            var nextRandom = random.Next(100);

            return nextRandom > 80 ? "Unsuccessful" : "Successful";
        }
    }
}