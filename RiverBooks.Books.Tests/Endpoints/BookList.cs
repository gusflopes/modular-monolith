using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace RiverBooks.Books.Tests.Endpoints;

public class BookList(Fixture fixture) : TestBase<Fixture>
{
  [Fact]
  public async Task ReturnThreeBooksAsync()
  {
    var testResult = await fixture.Client.GETAsync<List, ListBooksResponse>();

    testResult.Response.EnsureSuccessStatusCode();
    testResult.Result.Books.Count.Should().Be(3);

    // var response = await Client.GetAsync("/books");
    // response.EnsureSuccessStatusCode();
    // var content = await response.Content.ReadAsStringAsync();
    // var books = JsonSerializer.Deserialize<ListBooksResponse>(content);
    // Assert.NotNull(books);
    // Assert.NotEmpty(books.Books);
  }  
}
