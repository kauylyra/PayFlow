using AutoMapper;
using PayFlow.application.Crosscuttint.Dtos;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;
using PayFlow.application.Interfaces.Payment;
using PayFlow.domain.Interfaces.Customer;
using PayFlow.domain.Interfaces.Payment;
using PayFlow.domain.Models;
using PayFlow.domain.Models.Payment;

namespace PayFlow.application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICustomerRepository _customerFlowRepository;
    private readonly IMapper _mapper;
    public PaymentService(IPaymentRepository payFlowRepository, ICustomerRepository customerRepository, IMapper mapper)
    {
        _paymentRepository = payFlowRepository;
        _customerFlowRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<int> AddPaymentAsync(AddPaymentRequest dados)
    {
        var customer = await _customerFlowRepository.GetCustomerByIdAsync(dados.CustomerId);
        if (customer is null)
            throw new ExceptionValidation("Customer não encontrado.");

        var methodPayment = await _paymentRepository.GetPaymentByMethodIdAsync(dados.PaymentMethodId);
        if (methodPayment is null)
            throw new ExceptionValidation("Pagamento não encontrado.");

        if (dados.Amount <= 0)
            throw new ExceptionValidation("Customer não encontrado.");
        if (dados.Amount != Math.Round(dados.Amount, 2))
            throw new ExceptionValidation("O valor deve possuir no máximo 2 casas decimais.");

        var addPayment = _mapper.Map<Payments>(dados);
        addPayment.CreatedAt = DateTime.UtcNow;
        addPayment.Status = "PENDING";

        var ret = await _paymentRepository.AddPaymentAsync(addPayment);
        return ret;
    }

    public async Task<PaymentsResponse> GetPaymentByIdAsync(int id_payment)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(id_payment);
        if (payment is null)
            throw new ExceptionValidation("Pagamento não encontrado.");
        var ret = _mapper.Map<PaymentsResponse>(payment);
        return ret;
    }

    public async Task<PaginationResponse<IEnumerable<PaymentsResponse>>> GetPaymentAsync(
    FilterPaymentRequest request)
    {
        var currentPage = request.page ?? 1;
        var currentPageSize = request.pageSize ?? 20;

        var payments = await _paymentRepository.GetPaymentsAsync(
            new FilterPayment
            {
                customerId = request.customerId,
                paymentMethodId = request.paymentMethodId,
                status = request.status,
                page = currentPage,
                pageSize = currentPageSize
            });

        var totalRecords = await _paymentRepository.PaymentsCountAsync(_mapper.Map<FilterPayment>(request));

        var result = _mapper.Map<IEnumerable<PaymentsResponse>>(payments);

        return new PaginationResponse<IEnumerable<PaymentsResponse>>
        {
            Resultado = result,
            TamanhoPagina = currentPageSize,
            TotalRegistros = totalRecords,
            pagina = currentPage
        };
    }

    private bool IsValidStatusTransition(
    string currentStatus,
    string newStatus)
    {
        return currentStatus == "PENDING"
            && (newStatus == "APPROVED" || newStatus == "CANCELLED");
    }

    public async Task<bool> ApprovePaymentAsync(int id)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(id);

        if (payment == null)
            throw new KeyNotFoundException("Pagamento não encontrado.");

        if (!IsValidStatusTransition(payment.Status, "APPROVED"))
            throw new InvalidOperationException(
                "O pagamento não pode ser aprovado neste status.");

        return await _paymentRepository
            .UpdatePaymentStatusAsync(id, "APPROVED");
    }

    public async Task<bool> CancelPaymentAsync(int id)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(id);

        if (payment == null)
            throw new KeyNotFoundException("Pagamento não encontrado.");

        if (!IsValidStatusTransition(payment.Status, "CANCELLED"))
            throw new InvalidOperationException(
                "O pagamento não pode ser cancelado neste status.");

        return await _paymentRepository
            .UpdatePaymentStatusAsync(id, "CANCELLED");
    }
}
