using System;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Repositories;
using Checkout.PaymentsGateway.Domain.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Domain.UnitTests.Services
{
    public class CreatePaymentServiceShould
    {
        private Mock<IBankRepository> _bankRepositoryMock;
        private Mock<IPaymentsRepository> _paymentsRepositoryMock;
        private Mock<IAppLogger> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _bankRepositoryMock = new Mock<IBankRepository>();
            _paymentsRepositoryMock = new Mock<IPaymentsRepository>();
            _loggerMock = new Mock<IAppLogger>();
        }

        [Test]
        public async Task CreateTransaction_ShouldReturnTransactionId()
        {
            //Arrange
            var bankTransaction = GetBankTransactionObject();
            var bankTransactionId = Guid.NewGuid();
            var bankTransactionResponse = new TransactionResult()
            {
                TransactionStatus = "success",
                TransactionId = bankTransactionId
            };

            _bankRepositoryMock
                .Setup(x => x.CreateTransactionAsync(It.IsAny<BankTransaction>()))
                .ReturnsAsync(bankTransactionResponse);

            var sut = new CreatePaymentService(_bankRepositoryMock.Object, _paymentsRepositoryMock.Object,
                _loggerMock.Object);

            //Act
            var result = await sut.CreateTransaction(bankTransaction);

            //Assert
            result.Should().BeEquivalentTo(bankTransactionResponse);
        }

        [Test]
        public async Task CreateTransaction_ShouldRequestTheCreationOfABankTransaction()
        {
            //Arrange
            var bankTransaction = GetBankTransactionObject();
            var bankTransactionId = Guid.NewGuid();
            var bankTransactionResponse = new TransactionResult()
            {
                TransactionStatus = "success",
                TransactionId = bankTransactionId
            };

            _bankRepositoryMock
                .Setup(x => x.CreateTransactionAsync(It.IsAny<BankTransaction>()))
                .ReturnsAsync(bankTransactionResponse);

            var sut = new CreatePaymentService(_bankRepositoryMock.Object, _paymentsRepositoryMock.Object,
                _loggerMock.Object);

            //Act
            await sut.CreateTransaction(bankTransaction);

            //Assert
            _bankRepositoryMock.Verify(x => x.CreateTransactionAsync(bankTransaction), Times.Once);
        }

        [Test]
        public async Task CreateTransaction_ShouldLogBankTransaction()
        {
            //Arrange
            var bankTransaction = GetBankTransactionObject();
            var bankTransactionId = Guid.NewGuid();
            var bankTransactionResponse = new TransactionResult()
            {
                TransactionStatus = "success",
                TransactionId = bankTransactionId
            };

            _bankRepositoryMock
                .Setup(x => x.CreateTransactionAsync(It.IsAny<BankTransaction>()))
                .ReturnsAsync(bankTransactionResponse);

            var sut = new CreatePaymentService(_bankRepositoryMock.Object, _paymentsRepositoryMock.Object,
                _loggerMock.Object);

            //Act
            await sut.CreateTransaction(bankTransaction);

            //Assert
            _loggerMock.Verify(x =>
                    x.Write(LogLevel.Information,
                        $"{EventCodes.BankTransactionCreated} - TransactionId: {bankTransactionResponse.TransactionId}, TransactionStatus: {bankTransactionResponse.TransactionStatus}"),
                Times.Once);
        }

        [Test]
        public async Task CreateTransaction_ShouldRequestTheCreationOfAPaymentWithCorrectValues()
        {
            //Arrange
            var bankTransaction = GetBankTransactionObject();
            var bankTransactionId = Guid.NewGuid();
            var bankTransactionResult = new TransactionResult()
            {
                TransactionStatus = "success",
                TransactionId = bankTransactionId
            };

            _bankRepositoryMock
                .Setup(x => x.CreateTransactionAsync(It.IsAny<BankTransaction>()))
                .ReturnsAsync(bankTransactionResult);

            var passedPaymentRecord = new PaymentRecord();
            _paymentsRepositoryMock.Setup(x => x.AddPaymentAsync(It.IsAny<PaymentRecord>()))
                .Callback<PaymentRecord>((record) => passedPaymentRecord = record);

            var expectedPaymentRecord = GetPaymentRecordFromBankTransaction(bankTransactionResult, bankTransaction);

            var sut = new CreatePaymentService(_bankRepositoryMock.Object, _paymentsRepositoryMock.Object,
                _loggerMock.Object);

            //Act
            await sut.CreateTransaction(bankTransaction);

            //Assert
            _paymentsRepositoryMock.Verify(x => x.AddPaymentAsync(It.IsAny<PaymentRecord>()), Times.Once);
            expectedPaymentRecord.Should().BeEquivalentTo(passedPaymentRecord, config => config.Excluding(b => b.TransactionDate));
        }

        private PaymentRecord GetPaymentRecordFromBankTransaction(TransactionResult bankTransactionResult,
            BankTransaction bankTransaction)
        {
            return new PaymentRecord()
            {
                Id = bankTransactionResult.TransactionId,
                CompanyId = bankTransaction.CompanyId,
                Amount = bankTransaction.Amount,
                CardExpirationMonth = bankTransaction.CardExpirationMonth,
                CardExpirationYear = bankTransaction.CardExpirationYear,
                CardName = bankTransaction.CardHolderName,
                CardNumber = bankTransaction.CardNumber,
                Currency = bankTransaction.Currency,
                Cvv = bankTransaction.Cvv,
                TransactionStatus = bankTransactionResult.TransactionStatus,
                TransactionDate = DateTime.UtcNow
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
    }
}