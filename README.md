# blazor-wasm-base

1. Add Environment variables according to config classes.
1. Add Connection string to OnConfiguring method in MyDbContext.
1. Run "dotnet ef migrations add init" from dbrepository folder.
1. Run "dotnet ef database update" from dbrepository folder.