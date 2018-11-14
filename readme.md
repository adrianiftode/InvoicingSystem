
The connection string from the appsettings.json must be set/changed.
The database will be created at startup, unless `Migrate` has a different value.

There is a ready Postman collection that can be used for manual testing.

Some connection strings are hardcoded. These are used either at the dev time (DesignTimeDbContextFactory) or by an integration test that checks the migrations an a development machine, on a real relational database.

To do
- Cover notes by tests
- Repetitive code can be refactored (common Request class)
- better error handling as currently there is no way to tell why some action can't be completed
- the Update/Save methods can be extracted into a UoW
- instead of responding with Core.Models we could use some ViewModels