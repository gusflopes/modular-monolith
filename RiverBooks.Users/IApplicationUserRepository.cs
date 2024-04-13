namespace RiverBooks.Users;

public interface IApplicationUserRepository
{
  Task <ApplicationUser?> GetUserWithCartByEmail(string email);
  Task SaveChanges();
}
