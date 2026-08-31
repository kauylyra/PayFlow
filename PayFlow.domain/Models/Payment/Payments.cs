using PayFlow.Domain.Entities;

namespace PayFlow.domain.Models.Payment;

public class Payments
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
    public Customers Customer { get; set; } = null!;

    // Relacionamento N:1
    public PaymentMethod PaymentMethod { get; set; } = null!;
}