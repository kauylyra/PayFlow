using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PayFlow.application.Interfaces.Customer;
using PayFlow.application.Interfaces.Payment;
using PayFlow.tests.Fakes;

namespace PayFlow.tests.IntegrationTests;

public sealed class PayFlowWebApplicationFactory : WebApplicationFactory<Program>
{
    public FakeCustomerService CustomerService { get; } = new();
    public FakePaymentService PaymentService { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ICustomerService>();
            services.RemoveAll<IPaymentService>();

            services.AddSingleton<ICustomerService>(CustomerService);
            services.AddSingleton<IPaymentService>(PaymentService);
        });
    }
}
