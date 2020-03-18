using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Repositories;
using Checkout.PaymentsGateway.Infrastructure.Repositories;
using Checkout.PaymentsGateway.Infrastructure.Utils;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Infrastructure.UnitTests.Repositories
{
    public class PaymentsEncryptorRepositoryShould
    {
        private Mock<IPaymentsRepository> _innerPaymentsRepositoryMock;
        private Mock<IEncryptor> _encryptorMock;

        [SetUp]
        public void Setup()
        {
            _innerPaymentsRepositoryMock = new Mock<IPaymentsRepository>();
            _encryptorMock = new Mock<IEncryptor>();
        }

        [Test]
        public async Task AddPaymentAsync_ShouldEncryptCardNumberAndCardNameBeforeCallingInnerRepository()
        {
            //Arrange
            var paymentRecord = GetPaymentRecordObject();

            var expectedCardNumberEncryption = "cardNumberEncrypted";
            var expectedCardNameEncryption = "cardNameEncrypted";

            _encryptorMock.Setup(x => x.Encrypt(paymentRecord.CardNumber)).Returns(expectedCardNumberEncryption);
            _encryptorMock.Setup(x => x.Encrypt(paymentRecord.CardName)).Returns(expectedCardNameEncryption);

            var sut = new PaymentsEncryptorRepository(_innerPaymentsRepositoryMock.Object, _encryptorMock.Object);

            //Act
            await sut.AddPaymentAsync(paymentRecord);

            var expectedPaymentRecord = paymentRecord;

            expectedPaymentRecord.CardNumber = expectedCardNumberEncryption;
            expectedPaymentRecord.CardName = expectedCardNameEncryption;

            //Assert
            _innerPaymentsRepositoryMock.Verify(x => x.AddPaymentAsync(paymentRecord), Times.Once);
        }

        [Test]
        public async Task GetPaymentAsync_WhenInnerRepositoryResultIsNotNull_ShouldDecryptCardNumberAndCardName()
        {
            //Arrange
            var paymentRecord = GetPaymentRecordObject();

            var expectedCardNumberDecryption = "cardNumberDecrypted";
            var expectedCardNameDecryption = "cardNameDecrypted";

            _innerPaymentsRepositoryMock.Setup(x => x.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(paymentRecord);

            _encryptorMock.Setup(x => x.Decrypt(paymentRecord.CardNumber)).Returns(expectedCardNumberDecryption);
            _encryptorMock.Setup(x => x.Decrypt(paymentRecord.CardName)).Returns(expectedCardNameDecryption);

            var sut = new PaymentsEncryptorRepository(_innerPaymentsRepositoryMock.Object, _encryptorMock.Object);

            //Act
            var result = await sut.GetPaymentAsync(paymentRecord.Id, paymentRecord.CompanyId);

            var expectedPaymentRecord = paymentRecord;

            expectedPaymentRecord.CardNumber = expectedCardNumberDecryption;
            expectedPaymentRecord.CardName = expectedCardNameDecryption;

            //Assert
            result.Should().BeEquivalentTo(expectedPaymentRecord);
        }

        [Test]
        public async Task GetPaymentAsync_WhenInnerRepositoryResultIsNull_ShouldNotDecrypt()
        {
            //Arrange
            _innerPaymentsRepositoryMock.Setup(x => x.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(default(PaymentRecord?));

            var sut = new PaymentsEncryptorRepository(_innerPaymentsRepositoryMock.Object, _encryptorMock.Object);

            //Act
            var result = await sut.GetPaymentAsync(Guid.NewGuid(), Guid.NewGuid());

            //Assert
            _encryptorMock.Verify(x => x.Decrypt(It.IsAny<string>()), Times.Never);
            _encryptorMock.Verify(x => x.Encrypt(It.IsAny<string>()), Times.Never);
        }

        private PaymentRecord GetPaymentRecordObject()
        {
            return new PaymentRecord()
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                Amount = 200,
                CardExpirationMonth = "12",
                CardExpirationYear = "2020",
                CardName = "CardName",
                CardNumber = "4539252527166077",
                Currency = "GBP",
                Cvv = "123",
                TransactionStatus = "success",
                TransactionDate = DateTime.UtcNow
            };
        }

    }
}