using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PayFlow.application.Crosscuttint;
using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Customers;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;
using PayFlow.application.Interfaces.Customer;
using PayFlow.application.Services;

namespace PayFlow.web.V1.Controllers.Customer
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/customer/")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [Route("customers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<AddCustomerResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<Response<IEnumerable<AddCustomerResponse>>>> AddCustomerAsync(AddCustomersRequest dados)
        {
            var result = await _customerService.AddCustomerAsync(dados);
            return Ok(new Response<int> { result = result });
        }

        [HttpGet]
        [Route("customers/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CustomersResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<Response<CustomersResponse>>> GetCustomerByIdAsync(int id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            return Ok(new Response<CustomersResponse> { result = result });
        }

        [HttpGet]
        [Route("customers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CustomersResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<PaginationResponse<IEnumerable<CustomersResponse>>>> GetCustomersAsync(
            [FromQuery] int? page,
            [FromQuery] int? pageSize)
        {
            var result = await _customerService.GetCustomersAsync(page, pageSize);
            return Ok(new Response<PaginationResponse<IEnumerable<CustomersResponse>>> { result = result });
        }

        [HttpPut]
        [Route("customer{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<Response<bool>>> UpdateCustomerAsync(int id, UpdateCustomerRequest request)
        {
            var result = await _customerService.UpdateCustomerAsync(id, request);
            return Ok(new Response<bool> { result = result });
        }

        [HttpDelete]
        [Route("customer{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SingleResponse))]
        public async Task<ActionResult<Response<bool>>> DeleteCustomerAsync(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            return Ok(new Response<bool> { result = result });
        }
    }
}
