using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Crosscuttint.Dtos.Response.Customers;

namespace PayFlow.application.Interfaces.Customer;

public interface ICustomerService
{
    Task<int> AddCustomerAsync(AddCustomersRequest request);
    Task<CustomersResponse> GetCustomerByIdAsync(int id);
    Task<PaginationResponse<IEnumerable<CustomersResponse>>> GetCustomersAsync(int? page, int? pageSize);
    Task<bool> UpdateCustomerAsync(int id, UpdateCustomerRequest request);
    Task<bool> DeleteCustomerAsync(int id);
}
