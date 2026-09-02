namespace PayFlow.application.Crosscuttint.Dtos.Request.Payments;

public class FilterPaymentRequest
{
    public int? customerId { get; set; }
    public int? paymentMethodId { get; set; }
    public string? status { get; set; }
    public int? page { get; set; }
    public int? pageSize { get; set; }
}
