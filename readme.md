
The connection string from the appsettings.json must be set/changed.
The database will be created at startup, unless `Migrate` has a different value.

There is a ready Postman collection that can be used for manual testing.

Some connection strings are hardcoded. These are used either at the dev time (DesignTimeDbContextFactory) or by an integration test that checks the migrations an a development machine, on a real relational dabase.