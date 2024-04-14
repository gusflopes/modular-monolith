namespace RiverBooks.Users.UseCases;

public record UserAddressDto(Guid Id, string Street1, string Street2, string City, string State, string ZipCode, string Country);
