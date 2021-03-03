using Internship.TransactionService.Application.Repository.TransactionRepository;
using Internship.TransactionService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Internship.TransactionService.Infrastructure.Extentions
{
    public static class InfastructureExtentions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            return services;
        }
    }
}