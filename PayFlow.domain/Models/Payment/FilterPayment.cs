using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.domain.Models.Payment;

public class FilterPayment
{
    public int? customerId { get; set; }
    public int? paymentMethodId { get; set; }
    public string? status { get; set; }
    public int? page { get; set; }
    public int? pageSize { get; set; }
}
