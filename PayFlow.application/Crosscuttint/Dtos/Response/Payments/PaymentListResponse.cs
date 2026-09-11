using PayFlow.application.Crosscuttint.Dtos.Response.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.application.Crosscuttint.Dtos.Response.Payments;

public class PaymentListResponse
{
    public CustomerPaymentResponse Customer { get; set; } = null!;

    public IEnumerable<PaymentItemResponse> Payments { get; set; } = [];
}
