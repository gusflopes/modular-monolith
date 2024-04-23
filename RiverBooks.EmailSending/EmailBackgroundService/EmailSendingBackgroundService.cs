using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RiverBooks.EmailSending.EmailBackgroundService;

internal class EmailSendingBackgroundService : BackgroundService
{
  private readonly ILogger<EmailSendingBackgroundService> _logger;
  private readonly ISendEmailsFromOutboxService _service;

  public EmailSendingBackgroundService(ILogger<EmailSendingBackgroundService> logger, ISendEmailsFromOutboxService service)
  {
    _logger = logger;
    _service = service;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    int delayMilliseconds = 10_000; // 10 seconds
    _logger.LogInformation("{serviceName} starting...", nameof(EmailSendingBackgroundService));

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        await _service.CheckForAndSendEmails();
      }
      catch (Exception ex)
      {
        _logger.LogError("Error processing outbox: {message}", ex.Message);
      }
      finally
      {
        await Task.Delay(delayMilliseconds, stoppingToken);
      }
    }
    _logger.LogInformation("{serviceName} stopping.", nameof(EmailSendingBackgroundService));
  }
}
