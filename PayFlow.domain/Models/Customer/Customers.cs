using PayFlow.domain.Models.Payment;

namespace PayFlow.Domain.Entities;

public class Customers
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Document { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Relacionamento 1:N
    public ICollection<Payments> Payments { get; set; } = new List<Payments>();
}