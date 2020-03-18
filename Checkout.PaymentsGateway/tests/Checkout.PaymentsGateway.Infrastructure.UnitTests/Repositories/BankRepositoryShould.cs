using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.PaymentsGateway.Api.MappingProfiles;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Requests;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Responses;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Infrastructure.HttpClients;
using Checkout.PaymentsGateway.Infrastructure.Repositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Infrastructure.UnitTests.Repositories
{
    public class BankRepositoryShould
    {
        private Mock<IBankHttpClient> _bankHttpClientMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _bankHttpClientMock = new Mock<IBankHttpClient>();
            _mapper = MapperUtils.GetMapper(new Profile[] { new DomainToInfra() });
        }

        [Test]
        public async Task CreateTransactionAsync_ShouldCreateTransactionWithCorrectRequestMapping()
        {
            //Arrange
            var bankTransactionRequest = GetBankTransactionObject();
            var createBankTransactionRequest =
                GetCreateBankTransactionRequestFromBankTransaction(bankTransactionRequest);
            var createBankTransactionResponse = GetCreateBankTransactionResponse();

            var passedCreateTransactionRequest = new CreateBankTransactionRequest();
            _bankHttpClientMock.Setup(x => x.CreateTransactionAsync(It.IsAny<CreateBankTransactionRequest>()))
                .ReturnsAsync(createBankTransactionResponse)
                .Callback<CreateBankTransactionRequest>(x => passedCreateTransactionRequest = x);

            var sut = new BankRepository(_bankHttpClientMock.Object, _mapper);

            //Act
            await sut.CreateTransactionAsync(bankTransactionRequest);

            //Assert
            passedCreateTransactionRequest.Should().BeEquivalentTo(createBankTransactionRequest);
            _bankHttpClientMock.Verify(x => x.CreateTransactionAsync(passedCreateTransactionRequest), Times.Once);
        }

        [Test]
        public async Task CreateTransactionAsync_ShouldReturnTransactionResultFromCreateBankTransactionResponse()
        {
            //Arrange
            var bankTransactionRequest = GetBankTransactionObject();
            var createBankTransactionResponse = GetCreateBankTransactionResponse();
            var expectedTransactionResult =
                GetTransactionResultFromCreateBankTransactionResponse(createBankTransactionResponse);

            _bankHttpClientMock.Setup(x => x.CreateTransactionAsync(It.IsAny<CreateBankTransactionRequest>()))
                .ReturnsAsync(createBankTransactionResponse);

            var sut = new BankRepository(_bankHttpClientMock.Object, _mapper);

            //Act
            var result = await sut.CreateTransactionAsync(bankTransactionRequest);

            //Assert
            result.Should().BeEquivalentTo(expectedTransactionResult);
        }

        private static TransactionResult GetTransactionResultFromCreateBankTransactionResponse(CreateBankTransactionResponse transactionResponse)
        {
            return new TransactionResult()
            {
                TransactionStatus = transactionResponse.Status,
                TransactionId = new Guid(transactionResponse.TransactionId)
            };
        }

        private static CreateBankTransactionResponse GetCreateBankTransactionResponse()
        {
            return new CreateBankTransactionResponse()
            {
                Status = "success",
                TransactionId = Guid.NewGuid().ToString()
            };
        }

        private BankTransaction GetBankTransactionObject()
        {
            return new BankTransaction()
            {
                CompanyId = Guid.NewGuid(),
                CardHolderName = "CardHolderName",
                CardNumber = "4539252527166077",
                Amount = 200,
                Currency = "GBP",
                CardExpirationMonth = "12",
                CardExpirationYear = "2020",
                Cvv = "123"
            };
        }

        private CreateBankTransactionRequest GetCreateBankTransactionRequestFromBankTransaction(BankTransaction bankTransaction)
        {
            return new CreateBankTransactionRequest()
            {
                CardHolderName = bankTransaction.CardHolderName,
                CardNumber = bankTransaction.CardNumber,
                Amount = bankTransaction.Amount,
                Currency = bankTransaction.Currency,
                CardExpirationMonth = bankTransaction.CardExpirationMonth,
                CardExpirationYear = bankTransaction.CardExpirationYear,
                Cvv = bankTransaction.Cvv
            };
        }
    }
}