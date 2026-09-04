using AutoMapper;
using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Crosscuttint.Dtos.Response.Customers;
using PayFlow.application.Interfaces.Customer;
using PayFlow.domain.Interfaces.Customer;
using PayFlow.domain.Interfaces.Payment;
using PayFlow.domain.Models;
using PayFlow.Domain.Entities;

namespace PayFlow.application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    public CustomerService(ICustomerRepository customerRepository
        , IPaymentRepository paymentRepository
        , IMapper mapper)
    {
        _customerRepository = customerRepository;
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    #region Validators
    public async Task<bool> CustomerValidator(AddCustomersRequest request)
    {
        Helpers.Helpers.NameValidator(request.Name);
        Helpers.Helpers.EmailValidator(request.Email);
        if (!Helpers.Helpers.DocumentValidator(request.Document))
            throw new ExceptionValidation("Documento inválido.");

        if (await _customerRepository.DocumentExists(request.Document))
            throw new ExceptionValidation("Documento já cadastrado.");

        if (await _customerRepository.EmailExists(request.Email))
            throw new ExceptionValidation("Email já cadastrado.");

        return true;
    }
    #endregion


    public async Task<int> AddCustomerAsync(AddCustomersRequest request)
    {
        await CustomerValidator(request);
        var addCustomer = _mapper.Map<Customers>(request);
        var ret = await _customerRepository.AddCustomerAsync(addCustomer);

        return ret;
    }
    public async Task<CustomersResponse> GetCustomerByIdAsync(int id)
    {
        if (id == 0)
            throw new ExceptionValidation($"id inválido : {id}");

        var Customer = await _customerRepository.GetCustomerByIdAsync(id);

        var ret = _mapper.Map<CustomersResponse>(Customer);
        return ret;
    }

    public async Task<PaginationResponse<IEnumerable<CustomersResponse>>> GetCustomersAsync(int? page, int? pageSize)
    {
        var currentPage = page ?? 1;
        var currentPageSize = pageSize ?? 20;

        var customers = await _customerRepository.GetCustomersAsync(currentPage, currentPageSize);
        var customersCount = await _customerRepository.CustomersCountAsync();
        var ret = _mapper.Map<List<CustomersResponse>>(customers);

        return new PaginationResponse<IEnumerable<CustomersResponse>>()
        {
            Resultado = ret,
            TamanhoPagina = currentPageSize,
            TotalRegistros = customersCount,
            pagina = currentPage
        };
    }
    public async Task<bool> UpdateCustomerAsync(int id, UpdateCustomerRequest request)
    {
        if (id == 0)
            throw new ExceptionValidation($"id inválido : {id}");

        Helpers.Helpers.NameValidator(request.Name);
        Helpers.Helpers.DocumentValidator(request.Document);
        Helpers.Helpers.EmailValidator(request.Email);

        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        if (customer is null)
            throw new ExceptionValidation("Customer não encontrado.");

        if (await _customerRepository.EmailExistsOtherCustomer(id, request.Email))
            throw new ExceptionValidation($"Email cadastrado para outro usuário.");

        if (await _customerRepository.DocumentExistsOtherCustomer(id, request.Document))
            throw new ExceptionValidation($"Documento cadastrado para outro usuário.");

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Document = request.Document;

        await _customerRepository.UpdateCustomerAsync(customer);

        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        if (id == 0)
            throw new ExceptionValidation($"id inválido : {id}");

        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        if (customer is null)
            throw new ExceptionValidation("Customer não encontrado.");

        var paymentCustomer = await _paymentRepository.GetPaymentsByIdCustomerAsync(id);
        if (paymentCustomer is not null)
            throw new ExceptionValidation("Este Customer tem um pagamento registrado.");

        await _customerRepository.DeleteCustomerAsync(customer);

        return true;
    }
}
