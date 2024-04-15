﻿using System.Security.Claims;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using RiverBooks.Users.UseCases;

namespace RiverBooks.Users.CartEndpoints;

internal class AddItem : Endpoint<AddCartItemRequest>
{
  private readonly IMediator _mediator;
  
  public AddItem(IMediator mediator)
  {
    _mediator = mediator;
  }
  public override void Configure()
  {
    Post("/cart");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(AddCartItemRequest req, CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");

    var command = new AddItemToCartCommand(req.BookId, req.Quantity, emailAddress!);
    
    var result = await _mediator!.Send(command, ct);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync();
    }

    if (result.Status == ResultStatus.Invalid)
    {
      await SendResultAsync(result.ToMinimalApiResult());
    }
    else
    {
      await SendOkAsync();
    }
    
  }
}
