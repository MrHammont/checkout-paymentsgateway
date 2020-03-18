using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Checkout.PaymentsGateway.Infrastructure.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            var message =
                $"StatusCode: '{response.StatusCode}', Content: '{await response.Content.ReadAsStringAsync()}', RequestMessage: '{response.RequestMessage}'";

            throw new HttpRequestException(message);
        }

        public static bool IsUnauthorized(this HttpResponseMessage response)
        {
            return response.StatusCode == HttpStatusCode.Unauthorized;
        }

        public static async Task<T> Deserialize<T>(this HttpResponseMessage response, JsonSerializer jsonSerializer)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(responseStream);
            using var jsonReader = new JsonTextReader(streamReader);

            return jsonSerializer.Deserialize<T>(jsonReader);
        }
    }
}