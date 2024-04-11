## Criar migrations


dotnet ef migrations add InitialMigration -c BookDbContext -p ..\RiverBooks.Books\RiverBooks.Books.csproj -s .\RiverBooks.Web.csproj -o Data/Migrations


dotnet ef database update


Remover <InvariantGlobalization>true</InvariantGlobalization> do arquivo .csproj do projeto RiverBooks.Web



## Atualizar o ambiente
dotnet ef database update -- --environment Testing