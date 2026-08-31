using PayFlow.domain.Models.Payment;

namespace PayFlow.Domain.Entities;

public class PaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    // Relacionamento 1:N
    public ICollection<Payments> Payments { get; set; } = new List<Payments>();
}