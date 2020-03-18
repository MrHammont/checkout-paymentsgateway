using Checkout.Core.Logging;
using Checkout.PaymentsGateway.Domain.Repositories;
using Checkout.PaymentsGateway.Domain.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentsGateway.Domain.UnitTests.Services
{
    public class GetPaymentServiceShould
    {
        private Mock<IPaymentsRepository> _paymentsRepositoryMock;
        private Mock<IAppLogger> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _paymentsRepositoryMock = new Mock<IPaymentsRepository>();
            _loggerMock = new Mock<IAppLogger>();
        }

        [Test]
        public async Task GetPayment_ShouldRequestPaymentForTransactionIdAndCompanyId()
        {
            //Arrange
            var companyId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();

            var sut = new GetPaymentService(_paymentsRepositoryMock.Object, _loggerMock.Object);

            //Act
            await sut.GetPaymentAsync(paymentId, companyId);

            //Assert
            _paymentsRepositoryMock.Verify(x => x.GetPaymentAsync(paymentId, companyId), Times.Once);
        }

        [Test]
        public async Task GetPayment_ShouldReturnPaymentReturnedByRepository()
        {
            //Arrange
            var companyId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();

            var expectedPaymentRecord = GeneratePaymentRecord(companyId, paymentId);

            _paymentsRepositoryMock.Setup(x => x.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(expectedPaymentRecord);

            var sut = new GetPaymentService(_paymentsRepositoryMock.Object, _loggerMock.Object);

            //Act
            var result = await sut.GetPaymentAsync(paymentId, companyId);

            //Assert
            result.Should().BeEquivalentTo(expectedPaymentRecord);
        }


        [Test]
        public async Task GetPayment_WhenPaymentDoesNotExist_ShouldLogDetails()
        {
            //Arrange
            var companyId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();

            var sut = new GetPaymentService(_paymentsRepositoryMock.Object, _loggerMock.Object);

            //Act
            await sut.GetPaymentAsync(paymentId, companyId);

            //Assert
            _loggerMock.Verify(
                x => x.Write(LogLevel.Warning,
                    $"{EventCodes.PaymentRequestedNotFound} - TransactionId: {paymentId}, CompanyId: {companyId}"),
                Times.Once);
        }

        private PaymentRecord GeneratePaymentRecord(Guid companyId, Guid paymentId)
        {
            return new PaymentRecord
            {
                Id = paymentId,
                CompanyId = companyId,
                Amount = 200,
                CardExpirationMonth = "12",
                CardExpirationYear = "2020",
                CardName = "Marcos Hasbeen Here",
                CardNumber = "4539252527166077",
                Currency = "GBP",
                Cvv = "123",
                TransactionStatus = "Success",
                TransactionDate = DateTime.UtcNow
            };
        }
    }
}