using System;
using Checkout.PaymentsGateway.Api.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Api.UnitTests.Extensions
{
    public class ServiceCollectionExtensionsShould
    {
        private IServiceCollection _serviceCollection;

        [SetUp]
        public void Setup()
        {
            _serviceCollection = new ServiceCollection();
        }

        [Test]
        public void Decorate_WhenAlreadyAnImplementationExists_ShouldReplaceIt()
        {
            //Arrange
            _serviceCollection.AddSingleton<IDecorate, DecorateFoo>();

            //Act
            _serviceCollection.Decorate<IDecorate, DecorateBar>();

            //Assert
            _serviceCollection.BuildServiceProvider().GetService<IDecorate>().Should().BeOfType<DecorateBar>();
        }

        [Test]
        public void Decorate_WhenDecorating_ShouldInjectDecoratedInDecoratorObject()
        {
            //Arrange
            _serviceCollection.AddSingleton<IDecorate, DecorateFoo>();

            //Act
            _serviceCollection.Decorate<IDecorate, DecorateBar>();

            //Assert
            var decoratedService = _serviceCollection.BuildServiceProvider().GetService<IDecorate>();
            decoratedService.Should().BeOfType<DecorateBar>();

            ((DecorateBar) decoratedService).Foo.Should().BeOfType<DecorateFoo>();
        }

        [Test]
        public void Decorate_WhenNoPreviousImplementationExists_ShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                _serviceCollection.Decorate<IDecorate, DecorateBar>());
        }
    }

    public interface IDecorate
    {
    }

    public class DecorateFoo : IDecorate
    {
    }

    public class DecorateBar : IDecorate
    {
        public IDecorate Foo { get; private set; }

        public DecorateBar(IDecorate foo)
        {
            Foo = foo;
        }
    }
}