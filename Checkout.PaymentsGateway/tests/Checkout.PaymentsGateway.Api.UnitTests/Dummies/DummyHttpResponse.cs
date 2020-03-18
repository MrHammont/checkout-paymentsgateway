using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Checkout.PaymentsGateway.Api.UnitTests.Dummies
{
    public class DummyHttpResponse : HttpResponse
    {
        public DummyHttpResponse()
        {
            Headers = new HeaderDictionary();
        }

        private bool hasStarted = false;
        private Func<object, Task> callback;
        private object state;

        public Task InvokeCallBack()
        {
            hasStarted = true;
            return callback(state);
        }

        public override void OnCompleted(Func<object, Task> callback, object state)
        {
        }

        public override void OnStarting(Func<object, Task> callback, object state)
        {
            this.callback = callback;
            this.state = state;
        }

        public override void Redirect(string location, bool permanent)
        {
        }

        public override Stream Body { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override IResponseCookies Cookies { get; }
        public override bool HasStarted { get; }
        public override IHeaderDictionary Headers { get; }
        public override HttpContext HttpContext { get; }
        public override int StatusCode { get; set; }
    }
}