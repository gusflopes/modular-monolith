namespace RiverBooks.Users.CartEndpoints;

public interface IApplicationUserRepository
{
  Task <ApplicationUser?> GetUserWithCartByEmail(string email);
  Task SaveChanges();
}
