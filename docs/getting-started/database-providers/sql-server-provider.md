---
title: SQL Server Data Provider
layout: home
parent: Database Providers
nav_order: 100
---

# {{ page.title }}

The `LightNap.DataProviders.SqlServer` project contains the migrations for a SQL Server database. This project should never need to be manually edited as the migrations are handled by the `dotnet ef migrations` commands.

## Configuration

To use the SQL Server provider you will need to [configure `appsettings.json`](../application-configuration) or your deployment host with:

- `DatabaseProvider` set to `SqlServer`.
- `DefaultConnection` set to the connection string.

## Migrations

When the entity model changes it will be necessary to [create and apply a migration](./ef-migrations).
