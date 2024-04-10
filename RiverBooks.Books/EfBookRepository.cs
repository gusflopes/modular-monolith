using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Books;

internal class EfBookRepository : IBookRepository
{
  private readonly BookDbContext _dbContext;

  public EfBookRepository(BookDbContext dbContext)
  {
    _dbContext = dbContext;
  }
  public async Task Add(Book book)
  {
    await _dbContext.AddAsync(book);
  }
  public Task Delete(Book book)
  {
    _dbContext.Remove(book);
    return Task.CompletedTask;
  }
  public async Task<Book?> GetById(Guid id)
  {
    return await _dbContext!.Books.FindAsync(id);
  }

  public async Task<List<Book>> List()
  {
    return await _dbContext.Books.ToListAsync();
  }

  public Task SaveChanges()
  {
    throw new NotImplementedException();
  }
}
