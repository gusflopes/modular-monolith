using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RiverBooks.OrderProcessing.Integrations;

internal class ReadThroughOrderAddressCache : IOrderAddressCache
{
  // Depends on the RedisOrderAddressCache acting like a Decorator
  private readonly RedisOrderAddressCache _redisCache;
  private readonly IMediator _mediator;
  private readonly ILogger<ReadThroughOrderAddressCache> _logger;

  public ReadThroughOrderAddressCache(RedisOrderAddressCache redisCache, IMediator mediator, ILogger<ReadThroughOrderAddressCache> logger)
  {
    _redisCache = redisCache;
    _mediator = mediator;
    _logger = logger;
  }

  public async Task<Result<OrderAddress>> GetById(Guid addressId)
  {
    var result = await _redisCache.GetById(addressId);
    if (result.IsSuccess) return result;
    
    if (result.Status == ResultStatus.NotFound)
    {
      _logger.LogInformation("Address {id} not found; fetching from source.", addressId);
      
      var query  = new Users.Contracts.UserAddressDetailsByIdQuery(addressId);

      var queryResult = await _mediator.Send(query);

      if (queryResult.IsSuccess)
      {
        var dto = queryResult.Value;
        var address = new Address(dto.Street, dto.Street2, dto.City, dto.State, dto.ZipCode, dto.Country);

        var orderAddress = new OrderAddress(dto.AddressId, address);
        await Store(orderAddress);
        return orderAddress;
      }
    }
    return Result.NotFound();
  }

  public Task<Result> Store(OrderAddress orderAddress)
  {
    return _redisCache.Store(orderAddress);
  }
}
