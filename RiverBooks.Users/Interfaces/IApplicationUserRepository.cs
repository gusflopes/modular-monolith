﻿namespace RiverBooks.Users;

public interface IApplicationUserRepository
{
  Task <ApplicationUser?> GetUserWithCartByEmail(string email);
  Task <ApplicationUser?> GetUserWithAddressesByEmail(string email);
  Task <ApplicationUser?> GetUserById(Guid userId);
  Task SaveChanges();
}
