using FastEndpoints;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RiverBooks.OrderProcessing.Endpoints;

/// <summary>
/// Used for testing purposes only. Flushes the Redis cache.
/// </summary>
internal class FlushCache : EndpointWithoutRequest
{
  private readonly IDatabase _db;
  private readonly ILogger<FlushCache> _logger;

  public FlushCache(ILogger<FlushCache> logger)
  {
    var redis = ConnectionMultiplexer.Connect("localhost");
    _db = redis.GetDatabase();
    _logger = logger;
  }

  public override void Configure()
  {
    Delete("/orders/addresses");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    await _db.ExecuteAsync("FLUSHDB");
    _logger.LogWarning("FLUSHED CACHE FOR {db}", "REDIS");
  }
}
