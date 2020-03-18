using System;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.Api.Cache;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Api.UnitTests.Cache
{
    public class ResponseCacheServiceShould
    {
        private DistributedCacheMock _cacheMock;
        private Mock<IAppLogger> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _cacheMock = new DistributedCacheMock();
            _loggerMock = new Mock<IAppLogger>();
        }

        [Test]
        public async Task CacheResponseAsync_WhenEmptyCacheKeyIsPassed_ShouldNotCache()
        {
            //Arrange
            var cacheKey = string.Empty;
            var response = "test";
            var timeToLive = TimeSpan.FromSeconds(0);

            var sut = new ResponseCacheService(_cacheMock, _loggerMock.Object);

            //Act
            await sut.CacheResponseAsync(cacheKey, response, timeToLive);

            //Assert
            _cacheMock.SetStringAsyncWasCalled.Should().BeFalse();
        }

        [Test]
        public async Task CacheResponseAsync_WhenEmptyCacheKeyIsPassed_ShouldLogEvent()
        {
            //Arrange
            var cacheKey = string.Empty;
            var response = "test";
            var timeToLive = TimeSpan.FromSeconds(0);

            var sut = new ResponseCacheService(_cacheMock, _loggerMock.Object);

            //Act
            await sut.CacheResponseAsync(cacheKey, response, timeToLive);

            //Assert
            _loggerMock.Verify(x =>
                x.Write(LogLevel.Warning, EventCodes.CacheKeyMissingInCacheRequest), Times.Once);
        }

        [Test]
        public async Task CacheResponseAsync_WhenEmptyResponseIsPassed_ShouldNotCache()
        {
            //Arrange
            var cacheKey = "test";
            var response = default(object);
            var timeToLive = TimeSpan.FromSeconds(0);

            var sut = new ResponseCacheService(_cacheMock, _loggerMock.Object);

            //Act
            await sut.CacheResponseAsync(cacheKey, response, timeToLive);

            //Assert
            _cacheMock.SetStringAsyncWasCalled.Should().BeFalse();
        }

        [Test]
        public async Task CacheResponseAsync_WhenEmptyResponseIsPassed_ShouldLogEvent()
        {
            //Arrange
            var cacheKey = "test";
            var response = default(object);
            var timeToLive = TimeSpan.FromSeconds(0);

            var sut = new ResponseCacheService(_cacheMock, _loggerMock.Object);

            //Act
            await sut.CacheResponseAsync(cacheKey, response, timeToLive);

            //Assert
            _loggerMock.Verify(x =>
                x.Write(LogLevel.Warning, EventCodes.CacheObjectMissingInCacheRequest), Times.Once);
        }

        [Test]
        public async Task CacheResponseAsync_ShouldCache()
        {
            //Arrange
            var cacheKey = "test";
            var response = new {randomKey = "randomValue"};
            var timeToLive = TimeSpan.FromSeconds(1);

            var sut = new ResponseCacheService(_cacheMock, _loggerMock.Object);

            //Act
            await sut.CacheResponseAsync(cacheKey, response, timeToLive);

            //Assert
            _cacheMock.SetStringAsyncWasCalled.Should().BeTrue();
        }
    }
}