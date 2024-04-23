﻿using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;

namespace RiverBooks.EmailSending.EmailBackgroundService;

public class MimeKitEmailSender : ISendEmail
{
  private readonly ILogger<MimeKitEmailSender> _logger;

  public MimeKitEmailSender(ILogger<MimeKitEmailSender> logger)
  {
    _logger = logger;
  }

  public async Task SendEmail(string to, string from, string subject, string body)
  {
    _logger.LogInformation("Attempting to send email to {to} from {from} with subject {subject}", to, from, subject);

    using (var client = new SmtpClient())
    {
      client.Connect(Constants.EMAIL_SERVER, 25, false);
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(from,from));
      message.To.Add(new MailboxAddress(to,to));
      message.Subject = subject;
      message.Body = new TextPart("plain") { Text = body };
      await client.SendAsync(message);

      _logger.LogInformation("Email sent!");
      client.Disconnect(true);

    }

  }
}
