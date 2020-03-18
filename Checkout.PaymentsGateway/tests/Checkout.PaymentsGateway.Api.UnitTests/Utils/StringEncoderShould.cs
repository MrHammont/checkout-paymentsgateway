using Checkout.PaymentsGateway.Api.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Api.UnitTests.Utils
{
    public class StringEncoderShould
    {
        [Test]
        public void Encode_WhenNoValueIsPassed_ShouldReturnEmptyString()
        {
            //Arrange
            var sut = new StringEncoder();

            //Act
            var result = sut.Encode("");

            //Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void Encode_WhenAValueIsPassed_ShouldReturnEncodedValue()
        {
            //Arrange
            var value = "test_123_rkkr";
            var sut = new StringEncoder();

            //Act
            var result = sut.Encode(value);

            //Assert
            result.Should().NotBe(value);
        }
    }
}