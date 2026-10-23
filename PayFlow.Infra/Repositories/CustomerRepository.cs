using Microsoft.EntityFrameworkCore;
using PayFlow.domain.Interfaces.Customer;
using PayFlow.Domain.Entities;
using PayFlow.Infra.Context;

namespace PayFlow.Infra.Repositories;

public class CustomerRepository : DbContext, ICustomerRepository
{
    private readonly PayFlowDbContext _context;

    public CustomerRepository(PayFlowDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddCustomerAsync(Customers request)
    {
        request.CreatedAt = DateTime.UtcNow;
        await _context.Customers.AddAsync(request);
        await _context.SaveChangesAsync();

        return request.Id;
    }

    public async Task<bool> DocumentExists(string document)
    {
        return await _context.Customers.AnyAsync(x => x.Document == document);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _context.Customers.AnyAsync(x => x.Email == email);
    }

    public async Task<Customers> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers.Include(x => x.Payments).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> CustomersCountAsync()
    {
        return await _context.Customers.Include(x => x.Payments).CountAsync();
    }

    public async Task<IEnumerable<Customers>> GetCustomersAsync(int page, int pageSize)
    {
        return await _context.Customers.Include(x => x.Payments)
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<bool> EmailExistsOtherCustomer(int id, string email)
    {
        return await _context.Customers.AnyAsync(x => x.Email == email && x.Id != id);
    }

    public async Task<bool> DocumentExistsOtherCustomer(int id, string document)
    {
        return await _context.Customers.AnyAsync(x => x.Document == document && x.Id != id);
    }

    public async Task<bool> UpdateCustomerAsync(Customers request, CancellationToken cancellationToken)
    {
        request.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCustomerAsync(Customers request, CancellationToken cancellationToken)
    {
        _context.Customers.Remove(request);

        await _context.SaveChangesAsync();

        return true;
    }
}
