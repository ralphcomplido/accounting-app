---
title: Configuring Application Settings
layout: home
parent: Getting Started
nav_order: 300
---

# {{ page.title }}

Application settings need to be [configured in `appsettings.json`](./application-configuration) or your deployment host.

| File                                                                                                                                                                                        | Purpose                                                                                                                                                                                                       |
| :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `ApplicationSettings.AutomaticallyApplyEfMigrations`                                                                                                                                        | `true` to automatically apply Entity Framework migrations. If this is set to false, EF migrations must be manually applied. This setting is ignored for non-relational databases like the in-memory provider. |
| `ApplicationSettings.LogOutInactiveDeviceDays`                                                                                                                                              | The number of days of inactivity (no contact) before a device is logged out.                                                                                                                                  |
| `ApplicationSettings.RequireTwoFactorForNewUsers`                                                                                                                                           | `true` to require two-factor email authentication for new users. It does not change existing users.                                                                                                           |
| `ApplicationSettings.SiteUrlRootForEmails` | The root URL used in emails. This should be the base URL where the Angular app is hosted from (e.g., `https://localhost:4200/` in development). |
| `ApplicationSettings.UseSameSiteStrictCookies`                                                                                                                                              | `true` to use SameSite strict cookies. Set this to `false` if debugging the front-end from a different URL root (like `http://localhost:4200/` for Angular).                                                  |
