using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Requests;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Responses;
using Checkout.PaymentsGateway.Infrastructure.HttpClients;
using Checkout.PaymentsGateway.Infrastructure.Models;
using Checkout.PaymentsGateway.Infrastructure.UnitTests.Dummies;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Infrastructure.UnitTests.HttpClients
{
    public class BankHttpClientShould
    {
        private Mock<IAppLogger> _loggerMock;
        private Mock<HttpClient> _httpClientMock;
        private JsonSerializer _jsonSerializer;
        private BankClientOptions _BankClientOptions;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<IAppLogger>();
            _httpClientMock = new Mock<HttpClient>();
            _jsonSerializer = new JsonSerializer();
            _BankClientOptions = new BankClientOptions() {Url = "http://localhost"};
        }

        [Test]
        public async Task CreateTransactionAsync_WhenResponseIsUnauthorized_ShouldLogErrorMessageAndThrow()
        {
            //Arrange
            var createBankTransactionRequest = GetCreateBankTransactionRequestObject();
            var unauthorizedHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = Mock.Of<HttpContent>(),
                RequestMessage = Mock.Of<HttpRequestMessage>()
            };

            var unauthorizedHandler = new DummyHandler(() => Task.CompletedTask, unauthorizedHttpResponseMessage);

            var dummyClient = new HttpClient(unauthorizedHandler)
            {
                BaseAddress = new Uri(_BankClientOptions.Url)
            };

            var sut = new BankHttpClient(dummyClient, _loggerMock.Object, _jsonSerializer, _BankClientOptions);

            //Act
            Assert.ThrowsAsync<HttpRequestException>(async () =>
                await sut.CreateTransactionAsync(createBankTransactionRequest));

            //Assert
            _loggerMock.Verify(x =>
                    x.Write(LogLevel.Error, $"{EventCodes.ErrorCallingBankApi} - {It.IsAny<string>()}"),
                Times.Once);
        }

        [Test]
        public async Task CreateTransactionAsync_WhenResponseIsSuccess_ShouldReturnDeserializedContent()
        {
            //Arrange
            var createBankTransactionRequest = GetCreateBankTransactionRequestObject();
            var expectedTransactionResult = GetTransactionResultObject();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedTransactionResult)),
                RequestMessage = Mock.Of<HttpRequestMessage>()
            };

            var mockedHandler = new DummyHandler(() => Task.CompletedTask, httpResponseMessage);

            var dummyClient = new HttpClient(mockedHandler)
            {
                BaseAddress = new Uri(_BankClientOptions.Url)
            };

            var sut = new BankHttpClient(dummyClient, _loggerMock.Object, _jsonSerializer, _BankClientOptions);

            //Act
            var result = await sut.CreateTransactionAsync(createBankTransactionRequest);

            //Assert
            result.Should().BeEquivalentTo(expectedTransactionResult);
        }

        private CreateBankTransactionResponse GetTransactionResultObject()
        {
            return new CreateBankTransactionResponse
            {
                TransactionId = Guid.NewGuid().ToString(),
                Status = "Success"
            };
        }

        public CreateBankTransactionRequest GetCreateBankTransactionRequestObject()
        {
            return new CreateBankTransactionRequest
            {
                CardNumber = "4539252527166077",
                Amount = 200,
                CardExpirationMonth = "12",
                CardExpirationYear = "2020",
                CardHolderName = "CardHolderName",
                Currency = "GBP",
                Cvv = "123"
            };
        }
    }
}