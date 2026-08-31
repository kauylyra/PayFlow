using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.application.Crosscuttint.Dtos.Response.Payments;

public class AddPaymentResponse
{
    public int Id_Payment { get; set; }
    public bool status { get; set; }
}
