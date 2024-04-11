using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.Books;

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
  internal static readonly Guid Book1Guid = new Guid("741f0053-d934-4868-a890-f40217a5b1c5");
  internal static readonly Guid Book2Guid = new Guid("bf75e036-778c-4024-828f-7b8579c2e30c");
  internal static readonly Guid Book3Guid = new Guid("0c41afbb-a573-4f29-b4d1-c85d8951e0fb");

  public void Configure(EntityTypeBuilder<Book> builder)
  {
    builder.Property(p => p.Title)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();
    
    builder.Property(p => p.Author)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder.HasData(GetSampleBookData());
  }

  private IEnumerable<Book> GetSampleBookData()
  {
    var tolkien = "J.R.R. Tolkien";
    yield return new Book(Book1Guid, "The Fellowship of the Ring", tolkien, 9.99m);
    yield return new Book(Book2Guid, "The Two Towers", tolkien, 10.99m);
    yield return new Book(Book3Guid, "The Return of the King", tolkien, 11.99m);
  }
}
