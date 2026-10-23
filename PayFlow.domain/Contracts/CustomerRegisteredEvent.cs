namespace PayFlow.domain.Contracts;

public class CustomerRegisteredEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
