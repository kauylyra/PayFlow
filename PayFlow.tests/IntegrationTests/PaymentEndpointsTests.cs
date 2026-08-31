using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;

namespace PayFlow.tests.IntegrationTests;

public sealed class PaymentEndpointsTests : IClassFixture<PayFlowWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly PayFlowWebApplicationFactory _factory;

    public PaymentEndpointsTests(PayFlowWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostPayments_ShouldReturnCreatedPaymentId()
    {
        var request = new AddPaymentRequest
        {
            CustomerId = 10,
            Amount = 500,
            PaymentMethod = 1
        };

        var response = await _client.PostAsJsonAsync("/v1/payment/payments", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(_factory.PaymentService.AddPaymentResult, body.RootElement.GetProperty("result").GetInt32());
        Assert.Equal(request.CustomerId, _factory.PaymentService.LastAddRequest?.CustomerId);
    }
}
