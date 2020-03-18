using System;
using System.Text;

namespace Checkout.PaymentsGateway.Api.Utils
{
    public class StringEncoder : IStringEncoder
    {
        public string Encode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
            encoded = encoded.Replace("/", "_").Replace("+", "-");
            return encoded;
        }
    }
}