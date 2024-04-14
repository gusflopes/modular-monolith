using Ardalis.Result;
using Serilog;
using ILogger = Serilog.ILogger;

namespace RiverBooks.OrderProcessing.Integrations;

internal interface IOrderAddressCache
{
  Task<Result<OrderAddress>> GetById(Guid addressId);
  Task<Result> Store(OrderAddress orderAddress);
}
