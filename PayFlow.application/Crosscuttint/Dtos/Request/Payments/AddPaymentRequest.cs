using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.application.Crosscuttint.Dtos.Request.Payments
{
    public class AddPaymentRequest
    {
        public int CustomerId { get; set; }
        public int Amount { get; set; }
        public int PaymentMethod { get; set; }
    }
}
