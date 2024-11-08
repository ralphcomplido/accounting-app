---
title: SQLite Data Provider
layout: home
parent: Database Providers
nav_order: 200
---

# {{ page.title }}

The `LightNap.DataProviders.Sqlite` project contains the migrations for a SQLite database. This project should never need to be manually edited as the migrations are handled by the `dotnet ef migrations` commands.

## Configuration

To use the SQLite provider you will need to [configure `appsettings.json`](../application-configuration) or your deployment host with:

- `DatabaseProvider` set to `Sqlite`.
- `DefaultConnection` set to the connection string.

## Migrations

When the entity model changes it will be necessary to [create and apply a migration](./ef-migrations).
