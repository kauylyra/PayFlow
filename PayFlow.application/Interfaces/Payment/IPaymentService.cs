using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;

namespace PayFlow.application.Interfaces.Payment;

public interface IPaymentService
{
    /// <summary>
    /// Adiciona um pagamento
    /// </summary>
    /// <param name="dados"> Dados da requisição, passados pelo usuário </param>
    /// <returns> AddPaymentResponse </returns>
    Task<int> AddPaymentAsync(AddPaymentRequest dados);
}
