using AutoMapper;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;
using PayFlow.application.Interfaces.Payment;
using PayFlow.domain.Interfaces.Payment;
using PayFlow.domain.Models.Payment;
using PayFlow.Infra.Repositories;

namespace PayFlow.application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _payFlowRepository;
    private readonly IMapper _mapper;
    public PaymentService(IPaymentRepository payFlowRepository, IMapper mapper)
    {
        _payFlowRepository = payFlowRepository;
        _mapper = mapper;
    }

    public async Task<int> AddPaymentAsync(AddPaymentRequest dados)
    {
        var addPayment = _mapper.Map<Payments>(dados);
        var ret = await  _payFlowRepository.AddPaymentAsync(addPayment);
        return ret;
    }
}
