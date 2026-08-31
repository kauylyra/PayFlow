using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Interfaces.Payment;

namespace PayFlow.tests.Fakes;

public sealed class FakePaymentService : IPaymentService
{
    public AddPaymentRequest? LastAddRequest { get; private set; }
    public int AddPaymentResult { get; set; } = 20;

    public Task<int> AddPaymentAsync(AddPaymentRequest dados)
    {
        LastAddRequest = dados;
        return Task.FromResult(AddPaymentResult);
    }
}
