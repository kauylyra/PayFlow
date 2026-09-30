using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;
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

    public Task<int> AddPaymentAsync(AddPaymentRequest dados, string idempotencyKey)
        => AddPaymentAsync(dados);

    public Task<PaymentsResponse> GetPaymentByIdAsync(int idPayment) => Task.FromResult(new PaymentsResponse());

    public Task<PaginationResponse<PaymentListResponse>> GetPaymentAsync(FilterPaymentRequest request)
        => Task.FromResult(new PaginationResponse<PaymentListResponse>());

    public Task<bool> ApprovePaymentAsync(int id) => Task.FromResult(true);

    public Task<bool> CancelPaymentAsync(int id) => Task.FromResult(true);
}
