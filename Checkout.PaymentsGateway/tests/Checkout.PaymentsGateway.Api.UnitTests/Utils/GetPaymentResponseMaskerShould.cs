using Checkout.PaymentsGateway.Api.Utils;
using Checkout.PaymentsGateway.Domain.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Api.UnitTests.Utils
{
    public class GetPaymentResponseMaskerShould
    {
        [Test]
        public void MaskPaymentRecord_WhenCreditCardValueIsNullOrEmpty_ShouldNotMaskValue()
        {
            //Arrange
            var creditCardNumber = string.Empty;
            var paymentRecord = new PaymentRecord() {CardNumber = creditCardNumber};
            var sut = new GetPaymentResponseMasker();

            //Act
            var result = sut.MaskPaymentRecord(paymentRecord);

            //Assert
            result.CardNumber.Should().BeNullOrEmpty();
        }

        [TestCase("4539252527166077", "************6077")]
        [TestCase("4916761310092361364", "***************1364")]
        [TestCase("36243991963459", "**********3459")]
        [TestCase("345017258263969", "***********3969")]
        [TestCase("3450345", "***0345")]
        [TestCase("3450", "3450")]
        [TestCase("3", "3")]
        public void MaskPaymentRecord_WhenCreditCardValueIsProvided_ShouldShowLastFourDigitsAndMaskWithAsterisksTheRest(
            string actual, string expected)
        {
            //Arrange
            var paymentRecord = new PaymentRecord() {CardNumber = actual};
            var sut = new GetPaymentResponseMasker();

            //Act
            var result = sut.MaskPaymentRecord(paymentRecord);

            //Assert
            result.CardNumber.Should().Be(expected);
        }
    }
}