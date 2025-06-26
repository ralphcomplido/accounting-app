---
title: Updating the Sidebar Menu
layout: home
parent: Common Scenarios
nav_order: 700
---

# {{ page.title }}

The contents of the sidebar menu are managed in the `MenuService` at `app/layout/services/menu.service.ts`.

## How It Works

The service watches the status of the user (specifically whether they're logged in and whether they're an admin) in order to determine which menu items to include in the menu tree it publishes to subscribers.

There are three sections in the default menu:

- One for **Home** section.
- One for **Profile** section.
- One for **Admin** section.

{: .note}
Out of the box, users can't get to a view that includes the menu unless they're logged in. However, the menu is always generated to include the **Home** menu to illustrate how to provide menu items for anonymous users if you add publicly accessible pages that show the sidebar menu.

## Adding Menu Sections or Items

Most menu scenarios can be covered by just adding additional menu items following the pattern used in the codebase.

To add additional sections for new roles (like for a "moderator" scenario) the admin implementation should be followed. There is an `identityService.watchAnyUserRole$` method that accepts an array of allowed roles so that a list of roles (like `["Administrator", "Moderator"]`) can be checked.

Adding specific menu items within a section based on circumstances (like based on role) is a little more complicated because you'll need to do the legwork in `#refreshMenuItems`, but it's not overly difficult. It's also possible to watch navigation events to show certain menu sections or items based on where the user is in the app.
