using PayFlow.Domain.Entities;

namespace PayFlow.domain.Interfaces.Customer;

public interface ICustomerRepository
{
    Task<int> AddCustomerAsync(Customers request);
    Task<Customers> GetCustomerByIdAsync(int id);
    Task<IEnumerable<Customers>> GetCustomersAsync(int page, int pageSize);
    Task<int> CustomersCountAsync();
    Task<bool> DocumentExists(string document);
    Task<bool> EmailExists(string email);
    Task<bool> EmailExistsOtherCustomer(int id, string email);
    Task<bool> DocumentExistsOtherCustomer(int id, string document);
    Task<bool> UpdateCustomerAsync(Customers request, CancellationToken cancellationToken);
    Task<bool> DeleteCustomerAsync(Customers customer, CancellationToken cancellationToken);
}
