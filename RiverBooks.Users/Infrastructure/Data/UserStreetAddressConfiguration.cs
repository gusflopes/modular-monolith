using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.Users.Infrastructure.Data;

public class UserStreetAddressConfiguration : IEntityTypeConfiguration<UserStreetAddress>
{
  public void Configure(EntityTypeBuilder<UserStreetAddress> builder)
  {
    builder.Property(item => item.Id)
      .ValueGeneratedNever();

    builder.ComplexProperty(usa => usa.StreetAddress);
  }
}
