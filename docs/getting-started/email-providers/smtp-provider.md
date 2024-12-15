---
title: SMTP Provider
layout: home
parent: Email Providers
nav_order: 100
---

# {{ page.title }}

The SMTP email provider uses the configured SMTP settings to send email.

## Configuration

To use the SMTP provider you will need to [configure `Email` in `appsettings.json`](../configuring-application-settings) or your deployment host with:

- `Provider` set to `Smtp`.
- `Smtp.Host` set to the SMTP host.
- `Smtp.Port` set to the SMTP port.
- `Smtp.EnableSsl` set to `true` to use SSL.
- `Smtp.User` set to the SMTP username.
- `Smtp.Password` set to the SMTP password.
- `Smtp.FromEmail` set to the email to use for outbound messages.
- `Smtp.FromDisplayName` set to the display name to use for outbound messages.
