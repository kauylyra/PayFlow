using Humanizer.Localisation.DateToOrdinalWords;
using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;

namespace PayFlow.application.Interfaces.Payment;

public interface IPaymentService
{
    /// <summary>
    /// Adiciona um pagamento
    /// </summary>
    /// <param name="dados">Dados da requisição, passados pelo usuário</param>
    /// <returns>AddPaymentResponse</returns>
    Task<int> AddPaymentAsync(AddPaymentRequest dados);

    Task<PaymentsResponse> GetPaymentByIdAsync(int id_payment);

    Task<PaginationResponse<PaymentListResponse>> GetPaymentAsync(FilterPaymentRequest request);

    Task<bool> ApprovePaymentAsync(int id);

    Task<bool> CancelPaymentAsync(int id);
}
