using FastEndpoints;
using MongoDB.Driver;

namespace RiverBooks.EmailSending.ListEmailsEndpoint;

public class ListEmailResponse
{
  public int Count { get; set; }
  // Should use a DTO here instead of the Entity directly. Just simplified for study
  public List<EmailOutboxEntity> Emails { get; internal set; } = new();
}

internal class ListEmails : EndpointWithoutRequest<ListEmailResponse>
{
  private readonly IMongoCollection<EmailOutboxEntity> _emailCollection;

  public ListEmails(IMongoCollection<EmailOutboxEntity> emailCollection)
  {
    _emailCollection = emailCollection;
  }

  public override void Configure()
  {
    Get("/emails");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    // TODO: Implement paging
    var filter = Builders<EmailOutboxEntity>.Filter.Empty;
    var emailEntities = await _emailCollection.Find(filter)
      .ToListAsync(ct);

    var response = new ListEmailResponse()
    {
      Count = emailEntities.Count, Emails = emailEntities // TODO: Should use a separate DTO here
    };

    Response = response;
  }
}
