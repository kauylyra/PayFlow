namespace PayFlow.application.Crosscuttint.Dtos.Response.Payments;

public class PaymentMethodResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    // Relacionamento 1:N
    public ICollection<PaymentsResponse> Payments { get; set; } = new List<PaymentsResponse>();
}
