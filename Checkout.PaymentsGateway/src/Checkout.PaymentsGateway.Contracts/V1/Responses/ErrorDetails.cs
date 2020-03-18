﻿using Newtonsoft.Json;

namespace Checkout.PaymentsGateway.Contracts.V1.Responses
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}