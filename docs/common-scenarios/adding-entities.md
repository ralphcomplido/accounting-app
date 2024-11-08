---
title: Adding Entities
layout: home
parent: Common Scenarios
nav_order: 100
---

# {{ page.title }}

LightNap makes use of Entity Framework (EF) for database access. The `ApplicationDbContext` and supporting entities are all kept in the `Data` folder of the `LightNap.Core` project.

## Extending EF Models

Follow the common EF processes to ensure the changes are correctly integrated:

1. **Add/Update Entity Classes**: Create or edit new classes in the `Entities` folder that represent the new entities.
2. **Update DbContext**: Add `DbSet<TEntity>` properties to the `ApplicationDbContext` class for each new entity.
3. **Configure Relationships**: Define relationships and constraints using Fluent API or data annotations in your entity classes.
4. **Migrations**: Use [EF Core migrations](../getting-started/database-providers/ef-migrations) to update the database schema.

Once the new entity is wired up, check out [our scaffolding support](./scaffolding) to quickly generate code to expose that entity all the way through to the Angular app.
