﻿using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases;

namespace RiverBooks.Users.CartEndpoints;

public record CheckoutRequest(Guid ShippingAddressId, Guid BillingAddressId);
public record CheckoutResponse(Guid NewOrderId);


internal class Checkout : Endpoint<CheckoutRequest, CheckoutResponse>
{
  private readonly IMediator _mediator;

  public Checkout(IMediator mediator)
  {
    _mediator = mediator;
  }
  
  public override void Configure()
  {
    Post("/cart/checkout");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CheckoutRequest request, CancellationToken cancellationToken)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");
    var command = new CheckoutCartCommand(emailAddress!, request.ShippingAddressId, request.BillingAddressId);
    
    var result = await _mediator.Send(command);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync();
    }
    else
    {
      await SendOkAsync();
    }
  }
}
