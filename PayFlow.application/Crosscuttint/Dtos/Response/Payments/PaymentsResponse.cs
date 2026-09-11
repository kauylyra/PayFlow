using PayFlow.application.Crosscuttint.Dtos.Response.Customers;

namespace PayFlow.application.Crosscuttint.Dtos.Response.Payments;

public class PaymentsResponse
{
    public CustomerPaymentResponse Customer { get; set; } = null!;

    public IEnumerable<PaymentItemResponse> Payments { get; set; } = [];
}
