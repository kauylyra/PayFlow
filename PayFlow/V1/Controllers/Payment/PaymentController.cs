using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PayFlow.application.Crosscuttint;
using PayFlow.application.Crosscuttint.Dtos;
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
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService payFlowService)
        {
            _paymentService = payFlowService;
        }

        [HttpPost]
        [Route("payments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<AddPaymentResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<Response<IEnumerable<AddPaymentResponse>>>> AddPaymentAsync(
            AddPaymentRequest dados)
        {
            var resultado = await _paymentService.AddPaymentAsync(dados);

            return Ok(new Response<int>
            {
                result = resultado
            });
        }

        [HttpGet]
        [Route("payments/{id_payment}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<PaymentsResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<Response<PaymentsResponse>>> GetPaymentByIdAsync(int id_payment)
        {
            var resultado = await _paymentService.GetPaymentByIdAsync(id_payment);
            return Ok(new Response<PaymentsResponse>
            {
                result = resultado
            });
        }

        [HttpGet]
        [Route("payments")]
        [ProducesResponseType(
     StatusCodes.Status200OK,Type = typeof(Response<PaymentListResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(SingleResponse))]
        public async Task<ActionResult<PaginationResponse<PaymentListResponse>>> GetPaymentAsync([FromQuery] FilterPaymentRequest request)
        {
            var resultado = await _paymentService.GetPaymentAsync(request);

            return Ok(new Response<PaymentListResponse>{result = resultado.Resultado});
        }

        [HttpPost]
        [Route("payments/{id}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<Response<bool>>> ApprovePayment(int id)
        {
            var resultado = await _paymentService.ApprovePaymentAsync(id);
            return Ok(new Response<bool> { result = resultado });
        }

        [HttpPost]
        [Route("payments/{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<Response<bool>>> CancelPaymentAsync(int id)
        {
            var resultado = await _paymentService.CancelPaymentAsync(id);
            return Ok(new Response<bool> { result = resultado });
        }

    }
}
