---
title: In-Memory Provider
layout: home
parent: Database Providers
nav_order: 300
---

# {{ page.title }}

An in-memory database provider is included in the project to make it easier to develop and test the solution. Since the data itself is reset across sessions, it's not suitable for production use.

## Configuration

To use the In-Memory provider you will need to [configure `appsettings.json`](../application-configuration) or your deployment host with:

- `DatabaseProvider` set to `InMemory`.

## Migrations

You don't need to worry about migrations with the in-memory provider. When the app restarts, the database is reset. It's a convenient option to work with during development while designing the EF data model.
