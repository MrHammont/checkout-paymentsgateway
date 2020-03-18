using System;
using System.IO;
using Checkout.PaymentsGateway.DataContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Checkout.PaymentsGateway.DataContext
{
    public class PaymentsDb : DbContext
    {
        public PaymentsDb(DbContextOptions<PaymentsDb> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
    }

    public class PaymentsDbFactory : IDesignTimeDbContextFactory<PaymentsDb>
    {
        public PaymentsDb CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Checkout.PaymentsGateway.Api/appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<PaymentsDb>();
            var connectionString = configuration.GetConnectionString("PaymentsDbOptions:ConnectionString");
            builder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new PaymentsDb(builder.Options);
        }
    }
}