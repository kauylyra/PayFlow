using Microsoft.Extensions.DependencyInjection;
using PayFlow.application.Interfaces.Customer;
using PayFlow.application.Interfaces.Payment;
using PayFlow.application.Services;

namespace PayFlow.application.DI;

public class ConfigServices
{
    public static void ConfigureDependenciesServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICustomerService, CustomerService>();
        serviceCollection.AddScoped<IPaymentService, PaymentService>();
    }
}
