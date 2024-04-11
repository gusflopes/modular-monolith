using System.Runtime.CompilerServices;

// Necessário utilizar InternalsVisibleTo para permitir que os testes acessem classes internas,
// caso contrário os testes precisariam estar dentro do mesmo projeto

[assembly: InternalsVisibleTo("RiverBooks.Books.Tests")]
namespace RiverBooks.Books;

public class AssemblyInfo { }
