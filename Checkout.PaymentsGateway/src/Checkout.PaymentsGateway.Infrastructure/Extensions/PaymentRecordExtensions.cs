using System;
using Checkout.PaymentsGateway.Domain.Models;

namespace Checkout.PaymentsGateway.Infrastructure.Extensions
{
    public static class PaymentRecordExtensions
    {
        public static DateTime GetDateTimeFromCardDetails(this PaymentRecord record)
        {
            return new DateTime(int.Parse(record.CardExpirationYear), int.Parse(record.CardExpirationMonth), 1);
        }
    }
}