using Microsoft.AspNetCore.Mvc;
using PayFlow.application.Crosscuttint;
using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Crosscuttint.Dtos.Response.Customers;
using PayFlow.tests.Fakes;
using PayFlow.web.V1.Controllers.Customer;

namespace PayFlow.tests.UnitTests.Controllers;

public sealed class CustomerControllerTests
{
    private readonly FakeCustomerService _customerService = new();
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _controller = new CustomerController(_customerService);
    }

    [Fact]
    public async Task AddCustomerAsync_ShouldReturnCreatedCustomerId()
    {
        var request = CreateAddCustomerRequest();

        var response = await _controller.AddCustomerAsync(request);

        var result = Assert.IsType<OkObjectResult>(response.Result);
        var body = Assert.IsType<Response<int>>(result.Value);
        Assert.Equal(_customerService.AddCustomerResult, body.result);
        Assert.Same(request, _customerService.LastAddRequest);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnCustomer()
    {
        var response = await _controller.GetCustomerByIdAsync(10);

        var result = Assert.IsType<OkObjectResult>(response.Result);
        var body = Assert.IsType<Response<CustomersResponse>>(result.Value);
        Assert.Equal(10, body.result.Id);
        Assert.Equal(10, _customerService.LastRequestedId);
    }

    [Fact]
    public async Task GetCustomersAsync_ShouldReturnPagedCustomers()
    {
        var response = await _controller.GetCustomersAsync(2, 5);

        var result = Assert.IsType<OkObjectResult>(response.Result);
        var body = Assert.IsType<Response<IEnumerable<CustomersResponse>>>(result.Value);
        Assert.Single(body.result);
        Assert.Equal(2, _customerService.LastPage);
        Assert.Equal(5, _customerService.LastPageSize);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldReturnUpdateStatus()
    {
        var request = CreateUpdateCustomerRequest();

        var response = await _controller.UpdateCustomerAsync(10, request);

        var result = Assert.IsType<OkObjectResult>(response.Result);
        var body = Assert.IsType<Response<bool>>(result.Value);
        Assert.True(body.result);
        Assert.Equal(10, _customerService.LastRequestedId);
        Assert.Same(request, _customerService.LastUpdateRequest);
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldReturnDeleteStatus()
    {
        var response = await _controller.DeleteCustomerAsync(10);

        var result = Assert.IsType<OkObjectResult>(response.Result);
        var body = Assert.IsType<Response<bool>>(result.Value);
        Assert.True(body.result);
        Assert.Equal(10, _customerService.LastDeletedId);
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
