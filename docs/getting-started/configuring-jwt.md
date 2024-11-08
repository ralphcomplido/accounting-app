---
title: Configuring JSON Web Tokens (JWT)
layout: home
parent: Getting Started
nav_order: 600
---

# {{ page.title }}

Some JSON Web Token (JWT) parameters need to be [configured in `appsettings.json`](./application-configuration) or your deployment host. It will work out of the box as configured, but it's critical that the `Key` value be changed for at least production environments.

- `Jwt.Key`: The secret key used for JWT token generation. This can be any 32+ character string, such as a randomly generated GUID. It should be different across different environments (development vs. production and so on).
- `Jwt.Issuer`: The issuer of the JWT token. For typical scenarios this can be the site URL, such as `https://www.yourdomain.com`.
- `Jwt.Audience`: The audience of the JWT token. For typical scenarios this can be the site URL, such as `https://www.yourdomain.com`.
- `Jwt.ExpirationMinutes`: The expiration time of the JWT token in minutes. By default this is `120` minutes.
