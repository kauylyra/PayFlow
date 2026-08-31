using Microsoft.EntityFrameworkCore;
using PayFlow.domain.Interfaces.Payment;
using PayFlow.domain.Models.Payment;
using PayFlow.Infra.Context;

namespace PayFlow.Infra.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PayFlowDbContext _context;

    public PaymentRepository(PayFlowDbContext context)
    {
        _context = context;
    }

   

    public async Task<int> AddPaymentAsync(Payments request)
    {
        await _context.Payments.AddAsync(request);

        await _context.SaveChangesAsync();

        return request.Id;
    }

    public async Task<Payments> GetPaymentsByIdCustomerAsync(int customerId)
    {
        return await _context.Payments.Where(x => x.CustomerId == customerId).FirstOrDefaultAsync();
    }
}