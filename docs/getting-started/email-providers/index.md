---
title: Email Providers
parent: Getting Started
layout: home
nav_order: 500
---

# {{ page.title }}

Email functionality is provided at two levels, an `IEmailService` responsible for _what_ and an `IEmailSender` responsible for _how_ to send it.

## Configuration

You will need to [configure `appsettings.json`](../application-configuration) or your deployment host with several fields under the `Email` node:

- `Provider` defines the underlying sender mechanism to use (`LogToConsole` vs. `Smtp`).
- `FromEmail` is the email address messages are sent from.
- `FromDisplayName` is the display name messages are sent from.

Depending on the `Provider` selected there may be a need to configure further options, such as [`Smtp`](./smtp-provider) settings.
