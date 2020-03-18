using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.PaymentsGateway.Infrastructure.UnitTests.Dummies
{
    public class DummyHandler : DelegatingHandler
    {
        private readonly Func<Task> _onSend;
        private readonly HttpResponseMessage _fakeMessage;

        public DummyHandler(Func<Task> onSend, HttpResponseMessage fakeMessage = null)
        {
            _onSend = onSend;
            _fakeMessage = fakeMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await _onSend();

            if (_fakeMessage != null)
                return _fakeMessage;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}