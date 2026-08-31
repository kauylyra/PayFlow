using Microsoft.AspNetCore.Mvc;
using PayFlow.application.Crosscuttint;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.tests.Fakes;
using PayFlow.web.V1.Controllers.Pay;

namespace PayFlow.tests.UnitTests.Controllers;

public sealed class PaymentControllerTests
{
    private readonly FakePaymentService _paymentService = new();
    private readonly PaymentController _controller;

    public PaymentControllerTests()
    {
        _controller = new PaymentController(_paymentService);
    }

    [Fact]
    public async Task AddPaymentAsync_ShouldReturnCreatedPaymentId()
    {
        var request = new AddPaymentRequest
        {
            CustomerId = 10,
            Amount = 500,
            PaymentMethod = 1
        };

        var response = await _controller.AddPaymentAsync(request);

        var result = Assert.IsType<OkObjectResult>(response.Result);
        var body = Assert.IsType<Response<int>>(result.Value);
        Assert.Equal(_paymentService.AddPaymentResult, body.result);
        Assert.Same(request, _paymentService.LastAddRequest);
    }
}
