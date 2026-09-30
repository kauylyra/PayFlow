using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Services;
using PayFlow.domain.Interfaces.Customer;
using PayFlow.domain.Interfaces.Payment;
using PayFlow.domain.Models;
using PayFlow.domain.Models.Payment;
using PayFlow.Domain.Entities;

namespace PayFlow.tests.UnitTests.Services;

public sealed class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _payments = new();
    private readonly Mock<ICustomerRepository> _customers = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly PaymentService _service;

    public PaymentServiceTests()
    {
        _service = new PaymentService(_payments.Object, _customers.Object, _mapper.Object,
            NullLogger<PaymentService>.Instance);
    }

    [Fact]
    public async Task AddPaymentAsync_WhenCustomerDoesNotExist_ThrowsValidationException()
    {
        _customers.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync((Customers)null!);
        await Assert.ThrowsAsync<ExceptionValidation>(() => _service.AddPaymentAsync(Request()));
    }

    [Fact]
    public async Task AddPaymentAsync_WhenPaymentMethodDoesNotExist_ThrowsValidationException()
    {
        _customers.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync(new Customers());
        _payments.Setup(x => x.GetPaymentByMethodIdAsync(1)).ReturnsAsync((PaymentMethod)null!);
        await Assert.ThrowsAsync<ExceptionValidation>(() => _service.AddPaymentAsync(Request()));
    }

    [Fact]
    public async Task AddPaymentAsync_CreatesPendingPayment()
    {
        var payment = new Payments();
        _customers.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync(new Customers());
        _payments.Setup(x => x.GetPaymentByMethodIdAsync(1)).ReturnsAsync(new PaymentMethod());
        _mapper.Setup(x => x.Map<Payments>(It.IsAny<AddPaymentRequest>())).Returns(payment);
        _payments.Setup(x => x.AddPaymentAsync(payment)).ReturnsAsync(10);

        var id = await _service.AddPaymentAsync(Request());

        Assert.Equal(10, id);
        Assert.Equal("PENDING", payment.Status);
    }

    [Fact]
    public async Task ApprovePaymentAsync_ApprovesPendingPayment()
    {
        _payments.Setup(x => x.GetPaymentByIdAsync(1)).ReturnsAsync(new Payments { Id = 1, Status = "PENDING" });
        _payments.Setup(x => x.UpdatePaymentStatusAsync(1, "APPROVED")).ReturnsAsync(true);
        Assert.True(await _service.ApprovePaymentAsync(1));
    }

    [Fact]
    public async Task CancelPaymentAsync_CancelsPendingPayment()
    {
        _payments.Setup(x => x.GetPaymentByIdAsync(1)).ReturnsAsync(new Payments { Id = 1, Status = "PENDING" });
        _payments.Setup(x => x.UpdatePaymentStatusAsync(1, "CANCELLED")).ReturnsAsync(true);
        Assert.True(await _service.CancelPaymentAsync(1));
    }

    [Theory]
    [InlineData("APPROVED")]
    [InlineData("CANCELLED")]
    public async Task ApprovePaymentAsync_WithInvalidTransition_ThrowsConflict(string status)
    {
        _payments.Setup(x => x.GetPaymentByIdAsync(1)).ReturnsAsync(new Payments { Status = status });
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ApprovePaymentAsync(1));
    }

    [Fact]
    public async Task GetPaymentByIdAsync_WhenPaymentDoesNotExist_ThrowsNotFound()
    {
        _payments.Setup(x => x.GetPaymentByIdAsync(1)).ReturnsAsync((Payments)null!);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetPaymentByIdAsync(1));
    }

    private static AddPaymentRequest Request() => new() { CustomerId = 1, PaymentMethodId = 1, Amount = 10, Description = "Teste" };
}
