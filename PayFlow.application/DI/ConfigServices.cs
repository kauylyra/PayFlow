using Microsoft.Extensions.DependencyInjection;
using PayFlow.application.Interfaces.Customer;
using PayFlow.application.Interfaces.Payment;
using PayFlow.application.Services;
using PayFlow.Infra.Messaging;
using PayFlow.Infra.Messaging.Interface;

namespace PayFlow.application.DI;

public class ConfigServices
{
    public static void ConfigureDependenciesServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICustomerService, CustomerService>();
        serviceCollection.AddScoped<IPaymentService, PaymentService>();
        serviceCollection.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
    }
}
