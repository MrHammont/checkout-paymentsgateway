using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentsGateway.Api.Utils
{
    public static class DataSeeder
    {
        public static void SeedData<T>(this IApplicationBuilder app) where T : DbContext
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<T>();
            

            // Only accepted for development
            context.Database.Migrate();
        }
    }
}