using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PayFlow.application.Crosscuttint;
using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Crosscuttint.Dtos.Response.Customers;
using PayFlow.application.Interfaces.Customer;

namespace PayFlow.web.V1.Controllers.Customer;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/customer/")]
public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IValidator<AddCustomersRequest>? _addCustomerValidator;
    private readonly IValidator<UpdateCustomerRequest>? _updateCustomerValidator;

    public CustomerController(ICustomerService customerService, IValidator<AddCustomersRequest>? addCustomerValidator = null,
        IValidator<UpdateCustomerRequest>? updateCustomerValidator = null)
    {
        _customerService = customerService;
        _addCustomerValidator = addCustomerValidator;
        _updateCustomerValidator = updateCustomerValidator;
    }

    [HttpPost("customers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<AddCustomerResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<Response<IEnumerable<AddCustomerResponse>>>> AddCustomerAsync(AddCustomersRequest dados)
    {
        if (_addCustomerValidator is not null)
            await _addCustomerValidator.ValidateAndThrowAsync(dados);
        var result = await _customerService.AddCustomerAsync(dados);
        return Ok(new Response<int> { result = result });
    }

    [HttpGet("customers/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CustomersResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<Response<CustomersResponse>>> GetCustomerByIdAsync(int id)
    {
        var result = await _customerService.GetCustomerByIdAsync(id);
        return Ok(new Response<CustomersResponse> { result = result });
    }

    [HttpGet("customers")]
    public async Task<ActionResult<Response<IEnumerable<CustomersResponse>>>> GetCustomersAsync([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var result = await _customerService.GetCustomersAsync(page, pageSize);
        return Ok(new Response<IEnumerable<CustomersResponse>> { result = result.Resultado });
    }

    [HttpPut("customer/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<Response<bool>>> UpdateCustomerAsync(int id, UpdateCustomerRequest request)
    {
        if (_updateCustomerValidator is not null)
            await _updateCustomerValidator.ValidateAndThrowAsync(request);
        var result = await _customerService.UpdateCustomerAsync(id, request);
        return Ok(new Response<bool> { result = result });
    }

    [HttpDelete("customer/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<Response<bool>>> DeleteCustomerAsync(int id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        return Ok(new Response<bool> { result = result });
    }
}
