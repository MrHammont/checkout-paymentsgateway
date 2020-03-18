using System.Net;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.Api.Handlers;
using Checkout.PaymentsGateway.Api.UnitTests.MockUtils;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Api.UnitTests.Handlers
{
    public class GlobalExceptionHandlerShould
    {
        private HttpContext _httpContext;
        private Mock<IFeatureCollection> _featuresMock;
        private Mock<HttpResponse> _responseMock;
        private Mock<IAppLogger> _loggerMock;
        private Mock<IExceptionWriter> _exceptionWriterMock;
        private Mock<IExceptionHandlerFeature> _exceptionHandlerFeatureMock;

        [SetUp]
        public void Setup()
        {
            _exceptionWriterMock = new Mock<IExceptionWriter>();
            _loggerMock = new Mock<IAppLogger>();
            _featuresMock = new Mock<IFeatureCollection>();
            _responseMock = new Mock<HttpResponse>();
            _exceptionHandlerFeatureMock = new Mock<IExceptionHandlerFeature>();
        }

        [Test]
        public async Task Handle_WhenContextHasHandlerFeatureEnabled_ShouldWriteResponse()
        {
            //Arrange
            var errorMessage = "TestMessage";

            _exceptionHandlerFeatureMock
                .Setup(x => x.Error.Message)
                .Returns(errorMessage);

            _featuresMock
                .Setup(x => x.Get<IExceptionHandlerFeature>())
                .Returns(_exceptionHandlerFeatureMock.Object);

            _responseMock
                .SetupProperty(x => x.StatusCode)
                .SetupProperty(x => x.ContentType);

            _httpContext = new MockHttpContext(_featuresMock.Object, response: _responseMock.Object);

            var sut = new GlobalExceptionHandler(_exceptionWriterMock.Object, _loggerMock.Object);

            //Act
            await sut.Handle(_httpContext);

            //Assert
            _exceptionWriterMock.Verify(x => x.WriteResponse(_httpContext), Times.Once);
        }

        [Test]
        public async Task Handle_WhenContextHasHandlerFeatureEnabled_ShouldLogInternalServerError()
        {
            //Arrange
            var errorMessage = "TestMessage";

            _exceptionHandlerFeatureMock
                .Setup(x => x.Error.Message)
                .Returns(errorMessage);

            _featuresMock
                .Setup(x => x.Get<IExceptionHandlerFeature>())
                .Returns(_exceptionHandlerFeatureMock.Object);

            _httpContext = new MockHttpContext(_featuresMock.Object, response: _responseMock.Object);

            var sut = new GlobalExceptionHandler(_exceptionWriterMock.Object, _loggerMock.Object);

            //Act
            await sut.Handle(_httpContext);

            //Assert
            _loggerMock.Verify(x =>
                x.Write(LogLevel.Error, $"{EventCodes.InternalServerError} - {errorMessage}"), Times.Once);
        }

        [Test]
        public async Task Handle_WhenContextHasNoHandlerFeatureEnabled_ShouldSetResponseStatusCodeAndContentType()
        {
            //Arrange
            _featuresMock
                .Setup(x => x.Get<IExceptionHandlerFeature>())
                .Returns<IExceptionHandlerFeature>(null);

            _responseMock
                .SetupProperty(x => x.StatusCode)
                .SetupProperty(x => x.ContentType);

            _httpContext = new MockHttpContext(_featuresMock.Object, response: _responseMock.Object);

            var expectedBody = _responseMock.Object.Body;

            var sut = new GlobalExceptionHandler(_exceptionWriterMock.Object, _loggerMock.Object);

            //Act
            await sut.Handle(_httpContext);

            //Assert
            _responseMock.Object.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
            _responseMock.Object.ContentType.Should().Be("application/json");

            _exceptionWriterMock.Verify(x => x.WriteResponse(_httpContext), Times.Never);
        }
    }
}