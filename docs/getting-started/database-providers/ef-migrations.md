---
title: Entity Framework Migrations
layout: home
parent: Database Providers
nav_order: 400
---

# {{ page.title }}

Use the Entity Framework tools to create and apply migrations.

{: .note}

This page will use the `LightNap.DataProviders.SqlServer` migrations project, but the same commands should work by swapping that out for another provider, such as `LightNap.DataProviders.Sqlite`. There is no need to update migrations for database providers that will not be used. There is also no need to use migrations for the in-memory database provider.

The LightNap solution structure spreads out EF assets across multiple projects:

- Entities are defined in the `LightNap.Core` project.
- Migrations are defined in provider-specific projects, such as `LightNap.DataProviders.SqlServer`.
- The Web API entry point and EF configuration are defined in the `LightNap.WebApi` project.

As a result, additional parameters are required for the EF tools to work properly.

## Adding a Migration

To add a migration for SQL Server, use the following command from the `/src` folder:

```bash
dotnet ef migrations add <MigrationName> --context ApplicationDbContext --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
```

## Removing a Migration

Similarly, you can remove the most recent migration using the following command:

```bash
dotnet ef migrations remove --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
```

## Updating the Database

To apply changes to the database, use the following command:

```bash
dotnet ef database update --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
```

{: .note}

The `LightNap.WebApi` project also offers automatic migrations by setting
[`SiteSettings.AutomaticallyApplyEfMigrations`](../configuring-application-settings) to `true`.

## Regenerating the InitialCreate Migration

Prior to deploying a database it is common to tear down and regenerate the entire `InitialCreate` migration. To do this for SQL Server, use the following commands:

```bash
dotnet ef database drop --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
dotnet ef migrations remove --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
dotnet ef migrations add InitialCreate --context ApplicationDbContext --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
dotnet ef database update --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
```

{: .important}

That the `drop` command above will request confirmation to avoid inadvertently dropping the wrong database.
