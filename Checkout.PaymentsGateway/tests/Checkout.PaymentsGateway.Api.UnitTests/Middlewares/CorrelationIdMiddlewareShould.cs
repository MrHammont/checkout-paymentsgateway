using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Api.Middlewares;
using Checkout.PaymentsGateway.Api.Options;
using Checkout.PaymentsGateway.Api.UnitTests.Dummies;
using Checkout.PaymentsGateway.Api.UnitTests.MockUtils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Api.UnitTests.Middlewares
{
    public class CorrelationIdMiddlewareShould
    {
        private HttpContext _httpContext;
        private CorrelationIdOptions _cidOptions;
        private Mock<HttpRequest> _httpRequestMock;
        private Mock<HttpResponse> _httpResponseMock;

        [SetUp]
        public void Setup()
        {
            _httpRequestMock = new Mock<HttpRequest>();
            _httpResponseMock = new Mock<HttpResponse>();
            _cidOptions = new CorrelationIdOptions();
        }

        [Test]
        public async Task Invoke_WhenRequestHeadersContainCorrelationIdHeader_ShouldSetContextTraceIdentifierWithValue()
        {
            //Arrange
            var correlationIdHeader = "X-Correlation-ID";
            var correlationIdValue = Guid.NewGuid().ToString();

            _cidOptions.Header = correlationIdHeader;
            _cidOptions.IncludeInResponse = false;

            _httpRequestMock.Setup(x => x.Headers).Returns(() =>
                GetHeaderDictionary(new Tuple<string, string>(correlationIdHeader, correlationIdValue)));

            _httpContext = new MockHttpContext(request: _httpRequestMock.Object);

            var sut = new CorrelationIdMiddleware((innerHttpContext) => Task.FromResult(0), _cidOptions);

            //Act
            await sut.Invoke(_httpContext);

            //Assert
            _httpContext.TraceIdentifier.Should().Be(correlationIdValue);
        }


        [Test]
        public async Task
            Invoke_WhenRequestHeadersDoesNotContainCorrelationIdHeader_ShouldGenerateContextTraceIdentifierWithNewGuid()
        {
            //Arrange
            var correlationIdHeader = "X-Correlation-ID";
            var correlationIdValue = Guid.NewGuid().ToString();

            _cidOptions.Header = correlationIdHeader;
            _cidOptions.IncludeInResponse = false;

            _httpRequestMock.Setup(x => x.Headers).Returns(() =>
                GetHeaderDictionary(null));

            _httpContext = new MockHttpContext(request: _httpRequestMock.Object);

            var sut = new CorrelationIdMiddleware((innerHttpContext) => Task.FromResult(0), _cidOptions);

            //Act
            await sut.Invoke(_httpContext);

            //Assert
            _httpContext.TraceIdentifier.Should().NotBeEmpty();
            Guid.TryParse(_httpContext.TraceIdentifier, out var isGuid).Should().BeTrue();
        }


        [Test]
        public async Task Invoke_WhenOptionsIncludeInResponse_ShouldSetResponseHeaderWithCorrelationId()
        {
            //Arrange
            var correlationIdHeader = "X-Correlation-ID";
            var correlationIdValue = Guid.NewGuid().ToString();

            _cidOptions.Header = correlationIdHeader;
            _cidOptions.IncludeInResponse = true;

            _httpRequestMock.Setup(x => x.Headers).Returns(() =>
                GetHeaderDictionary(new Tuple<string, string>(correlationIdHeader, correlationIdValue)));

            _httpResponseMock.SetupGet(x => x.Headers).Returns(GetHeaderDictionary(null));

            var dummyRespond = new DummyHttpResponse();

            _httpContext = new MockHttpContext(request: _httpRequestMock.Object, response: dummyRespond);

            RequestDelegate next = async (ctx) => { await dummyRespond.InvokeCallBack(); };

            var sut = new CorrelationIdMiddleware(next, _cidOptions);

            //Act
            await sut.Invoke(_httpContext);

            //Assert
            _httpContext.Response.Headers.Count.Should().Be(1);
            _httpContext.Response.Headers.Should().ContainKey(correlationIdHeader);
            _httpContext.Response.Headers[correlationIdHeader].Should().BeEquivalentTo(correlationIdValue);
        }

        private static HeaderDictionary GetHeaderDictionary(Tuple<string, string> headerData)
        {
            var dict = new Dictionary<string, StringValues>();

            if (headerData != null)
                dict.Add(headerData.Item1, headerData.Item2);

            return new HeaderDictionary(dict);
        }
    }
}