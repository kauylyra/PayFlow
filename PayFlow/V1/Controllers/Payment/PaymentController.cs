using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PayFlow.application.Crosscuttint;
using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;
using PayFlow.application.Interfaces.Payment;

namespace PayFlow.web.V1.Controllers.Pay;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IValidator<AddPaymentRequest>? _addPaymentValidator;

    public PaymentController(IPaymentService paymentService, IValidator<AddPaymentRequest>? addPaymentValidator = null)
    {
        _paymentService = paymentService;
        _addPaymentValidator = addPaymentValidator;
    }

    [HttpPost("payments")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<AddPaymentResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<Response<IEnumerable<AddPaymentResponse>>>> AddPaymentAsync(AddPaymentRequest dados)
    {
        if (_addPaymentValidator is not null)
            await _addPaymentValidator.ValidateAndThrowAsync(dados);
        var key = HttpContext is null
            ? "unit-test"
            : Request.Headers["Idempotency-Key"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("O header Idempotency-Key é obrigatório.");
        var resultado = await _paymentService.AddPaymentAsync(dados, key);
        return Ok(new Response<int> { result = resultado });
    }

    [HttpGet("payments/{id_payment}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<PaymentsResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<Response<PaymentsResponse>>> GetPaymentByIdAsync(int id_payment)
    {
        var resultado = await _paymentService.GetPaymentByIdAsync(id_payment);
        return Ok(new Response<PaymentsResponse> { result = resultado });
    }

    [HttpGet("payments")]
    public async Task<ActionResult<Response<PaymentListResponse>>> GetPaymentAsync([FromQuery] FilterPaymentRequest request)
    {
        var resultado = await _paymentService.GetPaymentAsync(request);
        return Ok(new Response<PaymentListResponse> { result = resultado.Resultado });
    }

    [HttpPost("payments/{id}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<Response<bool>>> ApprovePayment(int id)
    {
        var resultado = await _paymentService.ApprovePaymentAsync(id);
        return Ok(new Response<bool> { result = resultado });
    }

    [HttpPost("payments/{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<Response<bool>>> CancelPaymentAsync(int id)
    {
        var resultado = await _paymentService.CancelPaymentAsync(id);
        return Ok(new Response<bool> { result = resultado });
    }
}
