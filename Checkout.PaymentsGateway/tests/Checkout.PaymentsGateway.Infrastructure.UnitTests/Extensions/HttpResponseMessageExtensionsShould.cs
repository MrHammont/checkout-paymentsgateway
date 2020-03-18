using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Infrastructure.Extensions;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Infrastructure.UnitTests.Extensions
{
    public class HttpResponseMessageExtensionsShould
    {
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.Conflict)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Ambiguous)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        [TestCase(HttpStatusCode.NotAcceptable)]
        [TestCase(HttpStatusCode.NotImplemented)]
        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        public async Task EnsureSuccessStatusCodeAsync_WhenResponseIsNotSuccessful_ShouldThrowHttpRequestException(
            HttpStatusCode code)
        {
            //Arrange
            var sut = new HttpResponseMessage(code)
            {
                Content = Mock.Of<HttpContent>()
            };

            //Act
            Assert.ThrowsAsync<HttpRequestException>(async () => await sut.EnsureSuccessStatusCodeAsync());
        }

        [Test]
        public void IsUnauthorized_WhenResponseStatusCodeIsUnauthorized_ShouldReturnTrue()
        {
            //Arrange
            var sut = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            //Act
            var result = sut.IsUnauthorized();

            result.Should().BeTrue();
        }
    }
}