namespace PayFlow.domain.Models.Payment;

public class PaymentIdempotency
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string RequestHash { get; set; } = string.Empty;
    public int PaymentId { get; set; }
    public DateTime CreatedAt { get; set; }
}
