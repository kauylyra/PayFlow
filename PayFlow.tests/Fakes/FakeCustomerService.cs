using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Crosscuttint.Dtos.Response.Customers;
using PayFlow.application.Interfaces.Customer;

namespace PayFlow.tests.Fakes;

public sealed class FakeCustomerService : ICustomerService
{
    public AddCustomersRequest? LastAddRequest { get; private set; }
    public UpdateCustomerRequest? LastUpdateRequest { get; private set; }
    public int? LastRequestedId { get; private set; }
    public int? LastDeletedId { get; private set; }
    public int? LastPage { get; private set; }
    public int? LastPageSize { get; private set; }

    public int AddCustomerResult { get; set; } = 10;
    public bool UpdateCustomerResult { get; set; } = true;
    public bool DeleteCustomerResult { get; set; } = true;
    public CustomersResponse CustomerResult { get; set; } = new()
    {
        Id = 10,
        Name = "Jane Doe",
        Email = "jane.doe@example.com",
        Document = "12345678901",
        CreatedAt = new DateTime(2026, 8, 31)
    };

    public PaginationResponse<IEnumerable<CustomersResponse>> CustomersResult { get; set; } = new()
    {
        Resultado = new[]
        {
            new CustomersResponse
            {
                Id = 10,
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Document = "12345678901",
                CreatedAt = new DateTime(2026, 8, 31)
            }
        },
        TamanhoPagina = 20,
        TotalRegistros = 1,
        pagina = 1
    };

    public Task<int> AddCustomerAsync(AddCustomersRequest request)
    {
        LastAddRequest = request;
        return Task.FromResult(AddCustomerResult);
    }

    public Task<CustomersResponse> GetCustomerByIdAsync(int id)
    {
        LastRequestedId = id;
        return Task.FromResult(CustomerResult);
    }

    public Task<PaginationResponse<IEnumerable<CustomersResponse>>> GetCustomersAsync(int? page, int? pageSize)
    {
        LastPage = page;
        LastPageSize = pageSize;
        return Task.FromResult(CustomersResult);
    }

    public Task<bool> UpdateCustomerAsync(int id, UpdateCustomerRequest request)
    {
        LastRequestedId = id;
        LastUpdateRequest = request;
        return Task.FromResult(UpdateCustomerResult);
    }

    public Task<bool> DeleteCustomerAsync(int id)
    {
        LastDeletedId = id;
        return Task.FromResult(DeleteCustomerResult);
    }
}
