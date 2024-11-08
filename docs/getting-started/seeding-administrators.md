---
title: Seeding Administrators
layout: home
parent: Getting Started
nav_order: 700
---

# {{ page.title }}

Administrator accounts can be [seeded in `appsettings.json`](./application-configuration) or your deployment host to help bootstrap the application.

- `Administrators`: List of administrator accounts to seed.
  - `Email`: The email address of the administrator.
  - `UserName`: The username of the administrator.
  - `Password`: The password of the administrator. If this field is blank or missing then a random password will be generated and the user will need to reset their password via the site to log in.

For example, the default configuration looks like this:

``` json
"Administrators": [
  {
    "Email": "admin@admin.com",
    "UserName": "admin",
    "Password": "A2m!nPassword"
  }
]
```

The seeder will loop through the list of configured administrators. For each it will check for an existing user with the specified email address and then create it if it does not yet exist. If the user already exists it will not confirm the provided user name matches or change the password.

Next, it will add the specified user to the built-in `Administrator` role. It will not remove existing administrators who are not in the list. As a result, administrators can be removed from the configuration after creation, if desired.
