namespace RiverBooks.OrderProcessing.Endpoints;

public record OrderSummary(Guid OrderId, Guid UserId, DateTimeOffset DateCreated, DateTimeOffset DateShipped, decimal Total);
