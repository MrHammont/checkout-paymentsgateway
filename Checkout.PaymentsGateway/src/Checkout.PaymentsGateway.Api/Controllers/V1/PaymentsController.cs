using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.PaymentsGateway.Api.Cache;
using Checkout.PaymentsGateway.Api.Extensions;
using Checkout.PaymentsGateway.Api.Services;
using Checkout.PaymentsGateway.Contracts.V1;
using Checkout.PaymentsGateway.Contracts.V1.Requests;
using Checkout.PaymentsGateway.Contracts.V1.Responses;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentsGateway.Api.Controllers.V1
{
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly ICreatePaymentService _createPaymentService;
        private readonly IGetPaymentService _getPaymentService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public PaymentsController(ICreatePaymentService createPaymentService,
            IGetPaymentService getPaymentService,
            IUriService uriService,
            IMapper mapper)
        {
            _createPaymentService = createPaymentService;
            _getPaymentService = getPaymentService;
            _uriService = uriService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Payments.Get)]
        [ProducesResponseType(typeof(Response<GetPaymentResponse>), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [Cached(60)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var companyId = HttpContext.GetCompanyId();
            var result = await _getPaymentService.GetPaymentAsync(id, companyId);

            var response = new Response<GetPaymentResponse>();
            if (result == null)
            {
                response.Status = "NotFound";
            }
            else
            {
                var data = _mapper.Map<GetPaymentResponse>(result);

                response.Status = "Success";
                response.Data = data;
            }

            return Ok(response);
        }

        [HttpPost(ApiRoutes.Payments.Post)]
        [ProducesResponseType(typeof(Response<TransactionResult>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> Post([FromBody] CreatePaymentRequest request)
        {
            var companyId = HttpContext.GetCompanyId();
            var transaction = _mapper.Map<BankTransaction>(request);
            transaction.CompanyId = companyId;

            var transactionResult = await _createPaymentService.CreateTransaction(transaction);

            var locationUri = _uriService.GetPaymentUri(transactionResult.TransactionId);
            return Created(locationUri, new Response<TransactionResult>
            {
                Data = transactionResult,
                Status = "Success"
            });
        }
    }
}