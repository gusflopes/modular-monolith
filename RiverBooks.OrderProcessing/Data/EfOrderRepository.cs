using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Data;

namespace RiverBooks.OrderProcessing;

internal class EfOrderRepository : IOrderRepository
{
  private readonly OrderProcessingDbContext _dbContext;

  public EfOrderRepository(OrderProcessingDbContext dbContext)
  {
    _dbContext = dbContext;
  }
  public async Task<List<Order>> List()
  {
    return await _dbContext.Orders
      .Include(o => o.OrderItems) // Checkout Ardalis.Specification for better way
      .ToListAsync();
  }

  public async Task Add(Order order)
  {
    await _dbContext.Orders.AddAsync(order);
  }

  public async Task SaveChanges()
  {
    await _dbContext.SaveChangesAsync();
  }
}
