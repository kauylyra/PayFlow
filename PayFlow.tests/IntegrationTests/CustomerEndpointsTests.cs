using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;

namespace PayFlow.tests.IntegrationTests;

public sealed class CustomerEndpointsTests : IClassFixture<PayFlowWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly PayFlowWebApplicationFactory _factory;

    public CustomerEndpointsTests(PayFlowWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostCustomers_ShouldReturnCreatedCustomerId()
    {
        var request = CreateAddCustomerRequest();

        var response = await _client.PostAsJsonAsync("/v1/customer/customers", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(_factory.CustomerService.AddCustomerResult, body.RootElement.GetProperty("result").GetInt32());
        Assert.Equal(request.Email, _factory.CustomerService.LastAddRequest?.Email);
    }

    [Fact]
    public async Task PostCustomers_WithInvalidRequest_ShouldReturnBadRequest()
    {
        var request = new AddCustomersRequest
        {
            Name = "Jo",
            Email = "invalid-email",
            Document = string.Empty
        };

        var response = await _client.PostAsJsonAsync("/v1/customer/customers", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnCustomer()
    {
        var response = await _client.GetAsync("/v1/customer/customers/10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(10, body.RootElement.GetProperty("result").GetProperty("id").GetInt32());
        Assert.Equal(10, _factory.CustomerService.LastRequestedId);
    }

    [Fact]
    public async Task GetCustomers_ShouldReturnPagedCustomers()
    {
        var response = await _client.GetAsync("/v1/customer/customers?page=2&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var pagination = body.RootElement.GetProperty("result");
        Assert.Equal(1, pagination.GetProperty("totalRegistros").GetInt32());
        Assert.Equal(2, _factory.CustomerService.LastPage);
        Assert.Equal(5, _factory.CustomerService.LastPageSize);
    }

    [Fact]
    public async Task PutCustomer_ShouldReturnUpdateStatus()
    {
        var request = CreateUpdateCustomerRequest();

        var response = await _client.PutAsJsonAsync("/v1/customer/customer10", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.True(body.RootElement.GetProperty("result").GetBoolean());
        Assert.Equal(10, _factory.CustomerService.LastRequestedId);
        Assert.Equal(request.Document, _factory.CustomerService.LastUpdateRequest?.Document);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldReturnDeleteStatus()
    {
        var response = await _client.DeleteAsync("/v1/customer/customer10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.True(body.RootElement.GetProperty("result").GetBoolean());
        Assert.Equal(10, _factory.CustomerService.LastDeletedId);
    }

    private static AddCustomersRequest CreateAddCustomerRequest()
    {
        return new AddCustomersRequest
        {
            Name = "Jane Doe",
            Email = "jane.doe@example.com",
            Document = "12345678901"
        };
    }

    private static UpdateCustomerRequest CreateUpdateCustomerRequest()
    {
        return new UpdateCustomerRequest
        {
            Name = "Jane Doe",
            Email = "jane.doe@example.com",
            Document = "12345678901"
        };
    }
}
