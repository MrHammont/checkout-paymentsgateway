using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.Infrastructure.Handlers;
using Checkout.PaymentsGateway.Infrastructure.UnitTests.Dummies;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Infrastructure.UnitTests.Handlers
{
    public class HttpExceptionHandlerShould
    {
        private Mock<IAppLogger> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<IAppLogger>();
        }

        [Test]
        public async Task SendAsync_WhenInnerHandlerThrowsAnException_ShouldLogErrorMessage()
        {
            //Arrange
            var exceptionMessage = "ExceptionMessage";
            var dummyHandlerForExceptionThrowing = new DummyHandler(() =>
                throw new Exception(exceptionMessage));

            var sut = new HttpExceptionHandler(_loggerMock.Object)
            {
                InnerHandler = dummyHandlerForExceptionThrowing
            };

            var httpClientMock = new HttpClient(sut)
            {
                BaseAddress = new Uri("http://localhost")
            };

            //Act
            Assert.ThrowsAsync<Exception>(async () =>
                await httpClientMock.SendAsync(Mock.Of<HttpRequestMessage>(), new CancellationToken()));

            //Assert
            _loggerMock.Verify(x =>
                    x.Write(LogLevel.Error, $"{EventCodes.ErrorCallingBankApi} - {exceptionMessage}"),
                Times.Once);
        }
    }
}