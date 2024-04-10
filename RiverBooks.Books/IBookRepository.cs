﻿namespace RiverBooks.Books;

internal interface IBookRepository : IReadOnlyBookRepository
{
  Task Add(Book book);
  Task Delete(Book book);
  Task SaveChanges();
}
