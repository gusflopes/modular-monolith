using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing;

internal interface IOrderRepository
{
  Task<List<Order>> List();
  Task Add(Order order);
  Task SaveChanges();
}
