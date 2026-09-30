using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Crosscuttint.Dtos.Response.Customers;
using PayFlow.application.Services;
using PayFlow.domain.Interfaces.Customer;
using PayFlow.domain.Interfaces.Payment;
using PayFlow.domain.Models;
using PayFlow.Domain.Entities;

namespace PayFlow.tests.UnitTests.Services;

public sealed class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _customers = new();
    private readonly Mock<IPaymentRepository> _payments = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _service = new CustomerService(_customers.Object, _payments.Object, _mapper.Object,
            NullLogger<CustomerService>.Instance);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WithInvalidId_ThrowsValidationException()
    {
        await Assert.ThrowsAsync<ExceptionValidation>(() => _service.GetCustomerByIdAsync(0));
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenCustomerDoesNotExist_ThrowsNotFound()
    {
        _customers.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync((Customers)null!);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetCustomerByIdAsync(1));
    }

    [Fact]
    public async Task AddCustomerAsync_WhenDocumentAlreadyExists_ThrowsValidationException()
    {
        _customers.Setup(x => x.DocumentExists(It.IsAny<string>())).ReturnsAsync(true);
        await Assert.ThrowsAsync<ExceptionValidation>(() => _service.AddCustomerAsync(Request()));
    }

    [Fact]
    public async Task AddCustomerAsync_WhenEmailAlreadyExists_ThrowsValidationException()
    {
        _customers.Setup(x => x.DocumentExists(It.IsAny<string>())).ReturnsAsync(false);
        _customers.Setup(x => x.EmailExists(It.IsAny<string>())).ReturnsAsync(true);
        await Assert.ThrowsAsync<ExceptionValidation>(() => _service.AddCustomerAsync(Request()));
    }

    [Fact]
    public async Task UpdateCustomerAsync_WhenEmailBelongsToAnotherCustomer_ThrowsValidationException()
    {
        _customers.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync(new Customers());
        _customers.Setup(x => x.EmailExistsOtherCustomer(1, It.IsAny<string>())).ReturnsAsync(true);
        await Assert.ThrowsAsync<ExceptionValidation>(() => _service.UpdateCustomerAsync(1, UpdateRequest()));
    }

    [Fact]
    public async Task UpdateCustomerAsync_UpdatesCustomer()
    {
        var customer = new Customers { Id = 1 };
        _customers.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync(customer);
        _customers.Setup(x => x.EmailExistsOtherCustomer(1, It.IsAny<string>())).ReturnsAsync(false);
        _customers.Setup(x => x.DocumentExistsOtherCustomer(1, It.IsAny<string>())).ReturnsAsync(false);

        var result = await _service.UpdateCustomerAsync(1, UpdateRequest());

        Assert.True(result);
        Assert.Equal("Jane Doe", customer.Name);
        _customers.Verify(x => x.UpdateCustomerAsync(customer), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomerAsync_WhenCustomerHasPayments_ThrowsConflict()
    {
        _customers.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync(new Customers());
        _payments.Setup(x => x.CustomerHasPaymentsAsync(1)).ReturnsAsync(true);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteCustomerAsync(1));
    }

    [Fact]
    public async Task DeleteCustomerAsync_DeletesCustomer()
    {
        var customer = new Customers { Id = 1 };
        _customers.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync(customer);
        _payments.Setup(x => x.CustomerHasPaymentsAsync(1)).ReturnsAsync(false);
        Assert.True(await _service.DeleteCustomerAsync(1));
        _customers.Verify(x => x.DeleteCustomerAsync(customer), Times.Once);
    }

    private static AddCustomersRequest Request() => new() { Name = "Jane Doe", Email = "jane@example.com", Document = "52998224725" };
    private static UpdateCustomerRequest UpdateRequest() => new() { Name = "Jane Doe", Email = "jane@example.com", Document = "52998224725" };
}
