using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;

namespace PayFlow.application.Interfaces.Payment;

public interface IPaymentService
{
    Task<int> AddPaymentAsync(AddPaymentRequest dados);
    Task<int> AddPaymentAsync(AddPaymentRequest dados, string idempotencyKey);
    Task<PaymentsResponse> GetPaymentByIdAsync(int idPayment);
    Task<PaginationResponse<PaymentListResponse>> GetPaymentAsync(FilterPaymentRequest request);
    Task<bool> ApprovePaymentAsync(int id);
    Task<bool> CancelPaymentAsync(int id);
}
