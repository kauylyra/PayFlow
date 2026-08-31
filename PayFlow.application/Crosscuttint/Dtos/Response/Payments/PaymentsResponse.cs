using PayFlow.application.Crosscuttint.Dtos.Response.Customers;

namespace PayFlow.application.Crosscuttint.Dtos.Response.Payments;

public class PaymentsResponse
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Relacionamento N:1
    public CustomersResponse Customers { get; set; } = null!;

    // Relacionamento N:1
    public PaymentMethodResponse PaymentMethod { get; set; } = null!;
}
