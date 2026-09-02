using PayFlow.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PayFlow.domain.Models.Payment;

public class Payments
{
    [Key]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Relacionamento N:1
    public Customers Customers { get; set; } = null!;

    // Relacionamento N:1
    public PaymentMethod PaymentMethod { get; set; } = null!;
}