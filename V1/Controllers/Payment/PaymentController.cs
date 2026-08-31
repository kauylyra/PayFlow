using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PayFlow.application.Crosscuttint;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;
using PayFlow.application.Interfaces.Payment;

namespace PayFlow.web.V1.Controllers.Pay
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _payFlowService;

        public PaymentController(IPaymentService payFlowService)
        {
            _payFlowService = payFlowService;
        }

        [HttpPost]
        [Route("payments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<AddPaymentResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<Response<IEnumerable<AddPaymentResponse>>>> AddPaymentAsync(
            AddPaymentRequest dados)
        {
            var resultado = await _payFlowService.AddPaymentAsync(dados);

            return Ok(new Response<int>
            {
                result = resultado
            });
        }
    }
}
