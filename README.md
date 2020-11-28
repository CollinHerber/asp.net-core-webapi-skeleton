# Net Core Example API

Net Core Example API

## Creating Database Migrations (Visual Studio):

- Open `Package Manager Console` (Tools -> Nuget Package Manager -> PMC)
- Change `Default project` to `src\Data`
- Execute `Add-Migration` command providing a name (Proper Case naming standard) for the database migration
 
```
    add-migration MigrationName
```

## Here are some steps to follow to get started from the command line:

Build API project
```
    cd "src/Api"
    dotnet publish --configuration Release
```
