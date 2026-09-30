using PayFlow.application.Crosscuttint.Dtos.Response.Payments;

namespace PayFlow.application.Crosscuttint.Dtos.Response.Customers;

public class CustomersResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Document { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ICollection<PaymentItemResponse> Payments { get; set; } = new List<PaymentItemResponse>();
}