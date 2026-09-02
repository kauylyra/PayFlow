using System.ComponentModel.DataAnnotations;

namespace PayFlow.application.Crosscuttint.Dtos.Request.Payments;

public class AddPaymentRequest
{
    [Required(ErrorMessage = "O customer é obrigatório.")]
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public int PaymentMethodId { get; set; }
    public string Description { get; set; }
}
