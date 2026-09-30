using Microsoft.EntityFrameworkCore;
using PayFlow.domain.Interfaces.Payment;
using PayFlow.domain.Models.Payment;
using PayFlow.Domain.Entities;
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

    public async Task<int> AddPaymentWithIdempotencyAsync(Payments payment, PaymentIdempotency idempotency)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
        idempotency.PaymentId = payment.Id;
        idempotency.CreatedAt = DateTime.UtcNow;
        await _context.PaymentIdempotencies.AddAsync(idempotency);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return payment.Id;
    }

    public Task<PaymentIdempotency?> GetPaymentIdempotencyAsync(string key, string operation)
    {
        return _context.PaymentIdempotencies.FirstOrDefaultAsync(x => x.Key == key && x.Operation == operation);
    }

    public async Task<Payments> GetPaymentByIdAsync(int id_payment)
    {
        return await _context.Payments.Include(x => x.Customers).Include(x => x.PaymentMethod).FirstOrDefaultAsync(x => x.Id == id_payment);
    }

    public async Task<PaymentMethod> GetPaymentByMethodIdAsync(int paymentMethodId)
    {
        return await _context.PaymentMethods.Where(x => x.Id == paymentMethodId).FirstOrDefaultAsync();
    }

    public async Task<bool> CustomerHasPaymentsAsync(int customerId)
    {
        return await _context.Payments.AnyAsync(x => x.CustomerId == customerId);
    }

    public async Task<int> PaymentsCountAsync(FilterPayment request)
    {
        var query = _context.Payments
            .AsNoTracking()
            .AsQueryable();

        if (request.customerId.HasValue)
            query = query.Where(x => x.CustomerId == request.customerId.Value);

        if (request.paymentMethodId.HasValue)
            query = query.Where(x => x.PaymentMethodId == request.paymentMethodId.Value);

        if (!string.IsNullOrWhiteSpace(request.status))
            query = query.Where(x => x.Status == request.status);

        return await query.CountAsync();
    }

    public async Task<IEnumerable<Payments>> GetPaymentsAsync(FilterPayment request)
    {
        var query = _context.Payments
            .Include(x=>x.Customers)
            .Include(x=>x.PaymentMethod)
            .AsNoTracking()
            .AsQueryable();

        if (request.customerId.HasValue)
            query = query.Where(x => x.CustomerId == request.customerId.Value);

        if (request.paymentMethodId.HasValue)
            query = query.Where(x => x.PaymentMethodId == request.paymentMethodId.Value);

        if (!string.IsNullOrWhiteSpace(request.status))
            query = query.Where(x => x.Status == request.status);

        return await query
            .OrderBy(x => x.Id)
            .Skip(((request.page ?? 1) - 1) * (request.pageSize ?? 20))
            .Take(request.pageSize ?? 20)
            .ToListAsync();
    }

    public async Task<bool> UpdatePaymentStatusAsync(int id, string status)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(x => x.Id == id);

        if (payment == null)
            return false;

        payment.Status = status;
        payment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
