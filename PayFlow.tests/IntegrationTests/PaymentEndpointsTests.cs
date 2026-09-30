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
        _client.DefaultRequestHeaders.Add("Idempotency-Key", "payment-endpoint-tests");
    }

    [Fact]
    public async Task PostPayments_ShouldReturnCreatedPaymentId()
    {
        var request = new AddPaymentRequest
        {
            CustomerId = 10,
            Amount = 500,
            PaymentMethodId = 1,
            Description = "Pagamento de teste"
        };

        var response = await _client.PostAsJsonAsync("/v1/payment/payments", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(_factory.PaymentService.AddPaymentResult, body.RootElement.GetProperty("result").GetInt32());
        Assert.Equal(request.CustomerId, _factory.PaymentService.LastAddRequest?.CustomerId);
    }

    [Fact]
    public async Task GetPaymentById_ShouldReturnPayment()
    {
        var response = await _client.GetAsync("/v1/payment/payments/10");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPayments_ShouldReturnPayments()
    {
        var response = await _client.GetAsync("/v1/payment/payments");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ApprovePayment_ShouldReturnSuccess()
    {
        var response = await _client.PostAsync("/v1/payment/payments/10/approve", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CancelPayment_ShouldReturnSuccess()
    {
        var response = await _client.PostAsync("/v1/payment/payments/10/cancel", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
