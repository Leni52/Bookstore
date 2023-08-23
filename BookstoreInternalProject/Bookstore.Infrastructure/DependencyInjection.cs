using Bookstore.Application.Common.Interfaces;
using Bookstore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Bookstore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookstoreContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Additional"));
            });

            services.AddScoped<IBookStoreContext>(ctx => ctx.GetRequiredService<BookstoreContext>());

            return services;
        }
    }

}
