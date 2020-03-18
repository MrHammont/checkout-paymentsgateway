using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.PaymentsGateway.DataContext;
using Checkout.PaymentsGateway.DataContext.Models;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentsGateway.Infrastructure.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly PaymentsDb _context;
        private readonly IMapper _mapper;

        public PaymentsRepository(PaymentsDb context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddPaymentAsync(PaymentRecord paymentRecord)
        {
            var payment = _mapper.Map<Payment>(paymentRecord);

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }


        public async Task<PaymentRecord?> GetPaymentAsync(Guid paymentId, Guid companyId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == paymentId && p.CompanyId == companyId);

            return payment == null ? null : _mapper.Map<PaymentRecord>(payment);
        }
    }
}