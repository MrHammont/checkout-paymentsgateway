using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Api.Proxy;
using Checkout.PaymentsGateway.Api.Utils;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Services;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Api.UnitTests.Proxy
{
    public class GetMaskedPaymentServiceShould
    {
        private Mock<IGetPaymentService> _paymentServiceMock;
        private Mock<IGetPaymentResponseMasker> _paymentResponseMaskerMock;

        [SetUp]
        public void Setup()
        {
            _paymentServiceMock = new Mock<IGetPaymentService>();
            _paymentResponseMaskerMock = new Mock<IGetPaymentResponseMasker>();
        }

        [Test]
        public async Task GetPayment_WhenPaymentResultIsNotNull_ShouldMaskCreditCardNumber()
        {
            //Arrange
            var innerPaymentResult = new PaymentRecord() {CardNumber = "4539252527166077"};

            _paymentServiceMock
                .Setup(x => x.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(innerPaymentResult);

            var sut = new GetMaskedPaymentService(_paymentServiceMock.Object, _paymentResponseMaskerMock.Object);

            //Act
            await sut.GetPaymentAsync(Guid.NewGuid(), Guid.NewGuid());

            //Assert
            _paymentResponseMaskerMock.Verify(x => x.MaskPaymentRecord(innerPaymentResult), Times.Once);
        }

        [Test]
        public async Task GetPayment_WhenPaymentResultIsNull_ShouldNotMaskCreditCardNumber()
        {
            //Arrange
            _paymentServiceMock
                .Setup(x => x.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(default(PaymentRecord?));

            var sut = new GetMaskedPaymentService(_paymentServiceMock.Object, _paymentResponseMaskerMock.Object);

            //Act
            await sut.GetPaymentAsync(Guid.NewGuid(), Guid.NewGuid());

            //Assert
            _paymentResponseMaskerMock.Verify(x => x.MaskPaymentRecord(It.IsAny<PaymentRecord>()), Times.Never);
        }
    }
}