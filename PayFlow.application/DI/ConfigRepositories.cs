using Microsoft.Extensions.DependencyInjection;
using PayFlow.domain.Interfaces.Customer;
using PayFlow.domain.Interfaces.Payment;
using PayFlow.Infra.Repositories;

namespace PayFlow.application.DI;

public class ConfigRepositories
{
    public static void ConfigureDependenciesRepositories(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICustomerRepository, CustomerRepository>();
        serviceCollection.AddScoped<IPaymentRepository, PaymentRepository>();
    }
}
