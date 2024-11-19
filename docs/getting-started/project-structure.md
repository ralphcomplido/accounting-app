---
title: Understanding the Project Structure
layout: home
parent: Getting Started
nav_order: 900
---

# {{ page.title }}

## Back-End Projects

- `LightNap.Core`: .NET shared library for common server-side components.
- `LightNap.Core.Tests`: Test library for `LightNap.Core`.
- `LightNap.DataProviders.Sqlite`: SQLite data provider implementation including migrations and utilities.
- `LightNap.DataProviders.SqlServer`: SQL Server data provider implementation including migrations and utilities.
- `LightNap.MaintenanceService`: .NET console project to run maintenance tasks.
- `LightNap.WebApi`: .NET Web API project.

## Front-End Project

- `lightnap-ng`: Angular project with PrimeNG components based on the [sakai-ng](https://github.com/primefaces/sakai-ng) template.

## Areas

The solution provides end-to-end infrastructure organized into five major areas:

1. **Identity**: Covers authentication, such as login, registration, password reset, and so on. Under typical scenarios these features will not require special attention from developers unless they are extending support for new scenarios, such as two-factor via SMS, OAuth login, etc.

2. **Profile**: Covers generic logged-in user features, such as setting profile data, managing devices & settings, and changing passwords. Extend this area to add new fields to the profile, such as first and last name, user avatar, etc.

3. **Administrator**: Covers administrative features, such as managing user roles, deleting users, and so on. This feature set is restricted to users who are in the `Administrator` role.

4. **Public**: Covers business features intended for any user, including those who are not logged in. This infrastructure is provided as a series of stubs to be extended for the application scenario.

5. **User**: Covers business features intended only for logged-in users. This infrastructure is also provided as a series of stubs to be extended for the application scenario.

## Data Flow

This solution is architected in a consistent way across all major areas. For any given feature it's implemented such that the end user interacts with Angular components that call into Angular services. Those services send HTTP requests to the REST endpoints hosted in Web API controllers. Those controllers relay requests to services that access the database via Entity Framework.

 ```mermaid
  graph TD
    subgraph Database
      DB[(Database)]
    end

    subgraph Back-End
      Core[LightNap.Core services]
      WebAPI[LightNap.WebApi controllers]
    end

    subgraph Front-End
      AngularService[lightnap-ng services]
      UIComponents[lightnap-ng components]
    end

    Core -.-> |Entity Framework| DB
    WebAPI --> Core
    AngularService -.-> |REST| WebAPI
    UIComponents --> AngularService
```

### Profile Example

Consider the scenario where a user navigates to their profile page.

 ```mermaid
  graph TD
    subgraph Database
      DB[(Database)]
    end

    subgraph Back-End
      Core["ProfileService"]
      WebAPI["ProfileController"]
    end

    subgraph Front-End
      DataService[DataService]
      ProfileService[ProfileService]
      ProfileComponent["(Profile) IndexComponent"]
    end

    Core -.-> |Entity Framework| DB
    WebAPI --> |"ProfileService.GetProfileAsync()"| Core
    DataService -.-> |GET /api/profile| WebAPI
    ProfileService --> |"DataService.getProfile()"| DataService
    ProfileComponent --> |"ProfileService.getProfile()"| ProfileService
```

This pattern is consistently applied across other areas such that understanding one complete area provides a head start on the others. For example, the back-end has an `IdentityController` that uses an `IdentityService`, an `AdministratorController` that uses an `AdministratorService`, and so on.
