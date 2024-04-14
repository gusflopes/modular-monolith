using System.Text.Json;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Serilog;
using ILogger = Serilog.ILogger;

namespace RiverBooks.OrderProcessing.Integrations;

internal interface IOrderAddressCache
{
  Task<Result<OrderAddress>> GetById(Guid addressId);
  Task<Result> Store(OrderAddress orderAddress);
}

internal class RedisOrderAddressCache : IOrderAddressCache
{
  private readonly ILogger<RedisOrderAddressCache> _logger;
  private readonly IDatabase _db;

  public RedisOrderAddressCache(ILogger<RedisOrderAddressCache> logger)
  {
    // TODO: Get server from Configuration
    var redis = ConnectionMultiplexer.Connect("localhost");
    _db = redis.GetDatabase();
    _logger = logger;
  }

  public async Task<Result<OrderAddress>> GetById(Guid addressId)
  {
    string? fetchedJson = await _db.StringGetAsync(addressId.ToString());

    if (fetchedJson is null)
    {
      _logger.LogWarning("Address {id} not found in {db}", addressId, "REDIS");
      return Result.NotFound();
    }

    var address = JsonSerializer.Deserialize<OrderAddress>(fetchedJson);

    if (address is null) return Result.NotFound();
    
    _logger.LogInformation("Address {id} returned from {db}", addressId, "REDIS");
    return Result.Success(address);
  }

  public async Task<Result> Store(OrderAddress orderAddress)
  {
    var key = orderAddress.Id.ToString();
    var addressJson = JsonSerializer.Serialize(orderAddress);
    
    await _db.StringSetAsync(key, addressJson);
    _logger.LogInformation("Adress {id} stored in {db}", orderAddress.Id, "REDIS");

    return Result.Success();
  }
}
