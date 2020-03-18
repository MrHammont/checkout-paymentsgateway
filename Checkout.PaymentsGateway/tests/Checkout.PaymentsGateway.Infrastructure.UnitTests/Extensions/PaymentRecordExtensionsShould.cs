using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Infrastructure.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Infrastructure.UnitTests.Extensions
{
    public class PaymentRecordExtensionsShould
    {
        [Test]
        public async Task GetDateTimeFromCardDetails_ShouldCreateDateTimeFromCardExpirationYearAndCardExpirationMonth()
        {
            //Arrange
            var month = "12";
            var year = "2020";
            var expectedDateTime = new DateTime(2020, 12, 1);

            var sut = GetPaymentRecord(month, year);

            //Act
            var result = sut.GetDateTimeFromCardDetails();

            //Assert
            result.Should().Be(expectedDateTime);
        }


        private PaymentRecord GetPaymentRecord(string month, string year)
        {
            return new PaymentRecord()
            {
                CardExpirationMonth = month,
                CardExpirationYear = year
            };
        }
    }
}