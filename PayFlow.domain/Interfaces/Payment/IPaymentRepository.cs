using PayFlow.domain.Models.Payment;

namespace PayFlow.domain.Interfaces.Payment;

public interface IPaymentRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<int> AddPaymentAsync(Payments request);

    Task<Payments> GetPaymentsByIdCustomerAsync(int customerId);
}
