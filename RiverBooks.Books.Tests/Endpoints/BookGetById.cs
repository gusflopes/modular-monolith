using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;

namespace RiverBooks.Books.Tests.Endpoints;

public class BookGetById(Fixture fixture) : TestBase<Fixture>
{
  [Theory]
  [InlineData("741f0053-d934-4868-a890-f40217a5b1c5", "The Fellowship of the Ring")]
  [InlineData("bf75e036-778c-4024-828f-7b8579c2e30c", "The Two Towers")]
  [InlineData("0c41afbb-a573-4f29-b4d1-c85d8951e0fb", "The Return of the King")]
  public async Task ReturnsExptectedBookGivenIdAsync(string validId, string expectedTitle)
  {
    Guid id = Guid.Parse(validId);
    var request = new GetBookByIdRequest(id);
    var testResult = await fixture.Client.GETAsync<GetById, GetBookByIdRequest, BookDto>(request);

    testResult.Response.EnsureSuccessStatusCode();
    testResult.Result.Title.Should().Be(expectedTitle);
  }
}
