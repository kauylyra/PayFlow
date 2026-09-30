using AutoMapper;
using Microsoft.Extensions.Logging;
using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Customers;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;
using PayFlow.application.Interfaces.Payment;
using PayFlow.domain.Interfaces.Customer;
using PayFlow.domain.Interfaces.Payment;
using PayFlow.domain.Models;
using PayFlow.domain.Models.Payment;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PayFlow.application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IPaymentRepository paymentRepository, ICustomerRepository customerRepository,
        IMapper mapper, ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> AddPaymentAsync(AddPaymentRequest dados)
    {
        if (await _customerRepository.GetCustomerByIdAsync(dados.CustomerId) is null)
            throw new ExceptionValidation("Customer não encontrado.");
        if (await _paymentRepository.GetPaymentByMethodIdAsync(dados.PaymentMethodId) is null)
            throw new ExceptionValidation("Método de pagamento não encontrado.");
        var payment = _mapper.Map<Payments>(dados);
        payment.CreatedAt = DateTime.UtcNow;
        payment.Status = "PENDING";
        var paymentId = await _paymentRepository.AddPaymentAsync(payment);
        _logger.LogInformation("Payment {PaymentId} criado com sucesso.", paymentId);
        return paymentId;
    }

    public async Task<int> AddPaymentAsync(AddPaymentRequest dados, string idempotencyKey)
    {
        const string operation = "POST:/v1/payment/payments";
        var requestHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(dados))));
        var existing = await _paymentRepository.GetPaymentIdempotencyAsync(idempotencyKey, operation);
        if (existing is not null)
        {
            if (existing.RequestHash != requestHash)
                throw new InvalidOperationException("A chave de idempotência já foi usada com outro payload.");
            return existing.PaymentId;
        }

        if (await _customerRepository.GetCustomerByIdAsync(dados.CustomerId) is null)
            throw new ExceptionValidation("Customer não encontrado.");
        if (await _paymentRepository.GetPaymentByMethodIdAsync(dados.PaymentMethodId) is null)
            throw new ExceptionValidation("Método de pagamento não encontrado.");

        var payment = _mapper.Map<Payments>(dados);
        payment.CreatedAt = DateTime.UtcNow;
        payment.Status = "PENDING";
        var id = await _paymentRepository.AddPaymentWithIdempotencyAsync(payment, new PaymentIdempotency
        {
            Key = idempotencyKey,
            Operation = operation,
            RequestHash = requestHash
        });
        _logger.LogInformation("Payment {PaymentId} criado com idempotência.", id);
        return id;
    }

    public async Task<PaymentsResponse> GetPaymentByIdAsync(int idPayment)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(idPayment);
        if (payment is null)
            throw new KeyNotFoundException("Pagamento não encontrado.");
        return _mapper.Map<PaymentsResponse>(payment);
    }

    public async Task<PaginationResponse<PaymentListResponse>> GetPaymentAsync(FilterPaymentRequest request)
    {
        var filter = new FilterPayment { customerId = request.customerId, paymentMethodId = request.paymentMethodId, status = request.status, page = request.page ?? 1, pageSize = request.pageSize ?? 20 };
        var payments = await _paymentRepository.GetPaymentsAsync(filter);
        var response = new PaymentListResponse
        {
            Customer = _mapper.Map<CustomerPaymentResponse>(payments.Select(x => x.Customers).FirstOrDefault()),
            Payments = _mapper.Map<List<PaymentItemResponse>>(payments)
        };
        return new PaginationResponse<PaymentListResponse>
        {
            Resultado = response,
            TamanhoPagina = filter.page.Value,
            TotalRegistros = await _paymentRepository.PaymentsCountAsync(filter),
            pagina = filter.page.Value
        };
    }

    public async Task<bool> ApprovePaymentAsync(int id) => await ChangePaymentStatusAsync(id, "APPROVED", "aprovado");

    public async Task<bool> CancelPaymentAsync(int id) => await ChangePaymentStatusAsync(id, "CANCELLED", "cancelado");

    private async Task<bool> ChangePaymentStatusAsync(int id, string status, string action)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(id);
        if (payment is null)
            throw new KeyNotFoundException("Pagamento não encontrado.");
        if (payment.Status != "PENDING")
            throw new InvalidOperationException($"O pagamento não pode ser {action} neste status.");
        var result = await _paymentRepository.UpdatePaymentStatusAsync(id, status);
        _logger.LogInformation("Payment {PaymentId} {Action}.", id, action);
        return result;
    }
}
