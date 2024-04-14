using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.Users.Infrastructure.Data;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
  public void Configure(EntityTypeBuilder<CartItem> builder)
  {
    builder.Property(item => item.Id)
      .ValueGeneratedNever();
  }
}
