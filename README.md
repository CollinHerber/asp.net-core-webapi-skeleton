# Chicks Gold API Server

Chicks Gold API Server platform code base.

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

Release API project
```
    sudo systemctl stop chicks-gold-api.service
    scp
    sudo systemctl start chicks-gold-api.service
```

View Log Files
```
    journalctl -fxeu chicks-gold-api.service
```
