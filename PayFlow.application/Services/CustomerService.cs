using AutoMapper;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<CustomerService> _logger;
    public CustomerService(ICustomerRepository customerRepository
        , IPaymentRepository paymentRepository
        , IMapper mapper
        , ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    #region Validators
    private async Task ValidateCustomerAsync(AddCustomersRequest request)
    {
        Helpers.Helpers.NameValidator(request.Name);
        if (!Helpers.Helpers.EmailValidator(request.Email))
            throw new ExceptionValidation("Email inválido.");

        if (!Helpers.Helpers.DocumentValidator(request.Document))
            throw new ExceptionValidation("Documento inválido.");

        if (await _customerRepository.DocumentExists(request.Document))
            throw new ExceptionValidation("Documento já cadastrado.");

        if (await _customerRepository.EmailExists(request.Email))
            throw new ExceptionValidation("Email já cadastrado.");
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ExceptionValidation($"Id inválido: {id}");
    }
    #endregion


    public async Task<int> AddCustomerAsync(AddCustomersRequest request)
    {
        await ValidateCustomerAsync(request);

        var customer = _mapper.Map<Customers>(request);

        var customerId = await _customerRepository.AddCustomerAsync(customer);

        _logger.LogInformation($"Customer {customerId} criado com sucesso.");

        return customerId;
    }

    public async Task<CustomersResponse> GetCustomerByIdAsync(int id)
    {
        ValidateId(id);

        var customer = await _customerRepository.GetCustomerByIdAsync(id);

        if (customer is null)
            throw new KeyNotFoundException("Customer não encontrado.");

        return _mapper.Map<CustomersResponse>(customer);
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
        ValidateId(id);

        Helpers.Helpers.NameValidator(request.Name);
        if (!Helpers.Helpers.EmailValidator(request.Email))
            throw new ExceptionValidation("Email inválido.");

        if (!Helpers.Helpers.DocumentValidator(request.Document))
            throw new ExceptionValidation("Documento inválido.");

        var customer = await _customerRepository.GetCustomerByIdAsync(id);

        if (customer is null)
            throw new KeyNotFoundException("Customer não encontrado.");

        if (await _customerRepository.EmailExistsOtherCustomer(id, request.Email))
            throw new ExceptionValidation(
                "Email cadastrado para outro usuário.");

        if (await _customerRepository.DocumentExistsOtherCustomer(id, request.Document))
            throw new ExceptionValidation(
                "Documento cadastrado para outro usuário.");

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Document = request.Document;

        await _customerRepository.UpdateCustomerAsync(customer);

        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        ValidateId(id);

        var customer = await _customerRepository.GetCustomerByIdAsync(id);

        if (customer is null)
            throw new KeyNotFoundException("Customer não encontrado.");

        if (await _paymentRepository.CustomerHasPaymentsAsync(id))
            throw new InvalidOperationException(
                "Não é possível excluir um Customer que possui pagamentos.");

        await _customerRepository.DeleteCustomerAsync(customer);

        return true;
    }
}
