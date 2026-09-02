using PayFlow.domain.Models.Payment;
using PayFlow.Domain.Entities;

namespace PayFlow.domain.Interfaces.Payment;

public interface IPaymentRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<int> AddPaymentAsync(Payments request);

    Task<Payments> GetPaymentByIdAsync(int id_payment);

    Task<PaymentMethod> GetPaymentByMethodIdAsync(int paymentMethodId);

    Task<Payments> GetPaymentsByIdCustomerAsync(int customerId);

    Task<IEnumerable<Payments>> GetPaymentsAsync(FilterPayment request);

    Task<int> PaymentsCountAsync(FilterPayment request);

    Task<bool> UpdatePaymentStatusAsync(int id, string status);
}
