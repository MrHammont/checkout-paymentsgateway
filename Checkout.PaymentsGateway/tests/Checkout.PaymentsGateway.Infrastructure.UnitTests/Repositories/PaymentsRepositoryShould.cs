using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.PaymentsGateway.Api.MappingProfiles;
using Checkout.PaymentsGateway.DataContext;
using Checkout.PaymentsGateway.DataContext.Models;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Checkout.PaymentsGateway.Infrastructure.UnitTests.Repositories
{
    public class PaymentsRepositoryShould
    {
        private PaymentsDb _context;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PaymentsDb>()
                .UseInMemoryDatabase(databaseName: "Payments")
                .Options;

            _context = new PaymentsDb(options);
            _mapper = MapperUtils.GetMapper(new Profile[] { new DomainToInfra(), new InfraToDomain() });
        }

        [Test]
        public async Task AddPaymentAsync_ShouldAddPaymentWithCorrectMappingToContext()
        {
            //Arrange
            var paymentRecord = GetPaymentRecordObject();
            var expectedPayment = GetPaymentFromPaymentRecord(paymentRecord);

            var sut = new PaymentsRepository(_context, _mapper);

            //Act
            await sut.AddPaymentAsync(paymentRecord);

            //Assert
            _context.Payments.Should().ContainEquivalentOf(expectedPayment);
        }

        [Test]
        public async Task GetPaymentAsync_WhenPaymentIdAndCompanyIdMatchInTheContext_ShouldReturnPayment()
        {
            //Arrange

            var expectedPaymentRecord = GetPaymentRecordObject();
            var paymentInContext = GetPaymentFromPaymentRecord(expectedPaymentRecord);

            _context.Payments.Add(paymentInContext);
            _context.SaveChanges();

            var sut = new PaymentsRepository(_context, _mapper);

            //Act
            var result = await sut.GetPaymentAsync(expectedPaymentRecord.Id, expectedPaymentRecord.CompanyId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedPaymentRecord);
        }

        [Test]
        public async Task GetPaymentAsync_WhenPaymentIdAndCompanyIdDoNotMatchInTheContext_ShouldReturnNull()
        {
            //Arrange
            var paymentRecord = GetPaymentRecordObject();
            var paymentInContext = GetPaymentFromPaymentRecord(paymentRecord);

            _context.Payments.Add(paymentInContext);
            _context.SaveChanges();

            var requestedPaymentId = Guid.NewGuid();
            var requestedCompanyId = Guid.NewGuid();

            var sut = new PaymentsRepository(_context, _mapper);

            //Act
            var result = await sut.GetPaymentAsync(requestedPaymentId, requestedCompanyId);

            //Assert
            result.Should().BeNull();
        }

        private Payment GetPaymentFromPaymentRecord(PaymentRecord paymentRecord)
        {
            return new Payment
            {
                Id = paymentRecord.Id,
                CompanyId = paymentRecord.CompanyId,
                Amount = paymentRecord.Amount,
                CardExpirationDate = new DateTime(int.Parse(paymentRecord.CardExpirationYear), int.Parse(paymentRecord.CardExpirationMonth), 1),
                CardName = paymentRecord.CardName,
                CardNumber = paymentRecord.CardNumber,
                Currency = paymentRecord.Currency,
                Cvv = paymentRecord.Cvv,
                TransactionStatus = paymentRecord.TransactionStatus,
                TransactionDate = paymentRecord.TransactionDate
            };
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