using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;

namespace Checkout.PaymentsGateway.Api.UnitTests.MockUtils
{
    public class MockHttpContext : HttpContext
    {
        public MockHttpContext(
            IFeatureCollection features = null,
            HttpRequest request = null,
            IServiceProvider requestServices = null,
            HttpResponse response = null,
            ClaimsPrincipal user = null)
        {
            Features = features ?? new Mock<IFeatureCollection>().Object;
            Request = request ?? new Mock<HttpRequest>().Object;
            RequestServices = requestServices ?? new Mock<IServiceProvider>().Object;
            Response = response ?? new Mock<HttpResponse>().Object;
            User = user ?? new Mock<ClaimsPrincipal>().Object;
        }

        public override void Abort()
        {
            throw new NotImplementedException();
        }

        public override ConnectionInfo Connection { get; }
        public override IFeatureCollection Features { get; }
        public override IDictionary<object, object> Items { get; set; }
        public override HttpRequest Request { get; }
        public override CancellationToken RequestAborted { get; set; }
        public override IServiceProvider RequestServices { get; set; }
        public override HttpResponse Response { get; }
        public override ISession Session { get; set; }
        public override string TraceIdentifier { get; set; }
        public override ClaimsPrincipal User { get; set; }
        public override WebSocketManager WebSockets { get; }
    }
}