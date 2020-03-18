using Checkout.PaymentsGateway.Api.Cache;
using Checkout.PaymentsGateway.Api.Utils;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Api.UnitTests.Cache
{
    public class CacheKeyProviderShould
    {
        private Mock<IStringEncoder> _encoderMock;

        [SetUp]
        public void Setup()
        {
            _encoderMock = new Mock<IStringEncoder>();
        }

        [Test]
        public void GenerateCacheKeyFromRequest_WhenNoParametersArePassed_ShouldReturnEmptyString()
        {
            //Arrange
            var sut = new CacheKeyProvider(_encoderMock.Object);

            //Act
            var result = sut.GenerateCacheKeyFromRequest();

            //Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void GenerateCacheKeyFromRequest_WhenParametersArePassed_ShouldConcatenateParameters()
        {
            //Arrange
            _encoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns<string>(val => val);
            var values = new[] {"test1", "test2"};
            var expectedValue = "test1_test2";
            var sut = new CacheKeyProvider(_encoderMock.Object);

            //Act
            var result = sut.GenerateCacheKeyFromRequest(values);

            //Assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void GenerateCacheKeyFromRequest_WhenParametersArePassed_ShouldEncodeConcatenatedValue()
        {
            //Arrange
            var values = new[] {"test1", "test2"};
            var expectedValuePassedToEncoder = "test1_test2";
            _encoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns<string>(val => val);
            var sut = new CacheKeyProvider(_encoderMock.Object);

            //Act
            sut.GenerateCacheKeyFromRequest(values);

            //Assert
            _encoderMock.Verify(x => x.Encode(expectedValuePassedToEncoder), Times.Once);
        }

        [Test]
        public void GenerateCacheKeyFromRequest_WhenParametersArePassed_ShouldReturnEncodedValue()
        {
            //Arrange
            var values = new[] {"test1", "test2"};
            var expectedValueFromEncoder = "asdasdwdawd";
            _encoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns<string>(val => expectedValueFromEncoder);
            var sut = new CacheKeyProvider(_encoderMock.Object);

            //Act
            var result = sut.GenerateCacheKeyFromRequest(values);

            //Assert
            result.Should().Be(expectedValueFromEncoder);
        }
    }
}