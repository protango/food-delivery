# Generic Food Delivery Company
*A completely original food delivery service*

## Database
### Configuration
The application expects a postgres database to be setup. You must provide a suitable postgres connection string to the application in the form of a user-secret, eg.
```
dotnet user-secrets set ConnectionStrings:FoodDelivery `
"Host=localhost;Database=FoodDelivery;Username=food-server;Password=***********"
```
Once the connection string is configured, you should initialise the database with the migration scripts, this can be done using
```
dotnet ef database update
```

### Migrations
Create a migration using:
```
dotnet ef migrations add [Migration Name]
```
Apply migrations to the database using:
```
dotnet ef database update
```
