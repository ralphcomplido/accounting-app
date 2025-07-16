import { AppRoute } from "@routing";

export const Routes: AppRoute[] = [
  { path: "", title: "Admin | Home", data: { alias: "admin-home" }, loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "users", title: "Admin | Users", data: { alias: "admin-users" }, loadComponent: () => import("./users/users.component").then(m => m.UsersComponent) },
  { path: "users/:userId", title: "Admin | User", data: { alias: "admin-user" }, loadComponent: () => import("./user/user.component").then(m => m.UserComponent) },
  { path: "roles", title: "Admin | Roles", data: { alias: "admin-roles" }, loadComponent: () => import("./roles/roles.component").then(m => m.RolesComponent) },
  { path: "roles/:role", title: "Admin | Role", data: { alias: "admin-role" }, loadComponent: () => import("./role/role.component").then(m => m.RoleComponent) },
  { path: "claims/:type/:value", title: "Admin | Claim", data: { alias: "admin-claim" }, loadComponent: () => import("./claim/claim.component").then(m => m.ClaimComponent) },
  { path: "account", title: "Admin | Account", data: { alias: "admin-account" }, loadComponent: () => import("./account/account.component").then(m => m.AccountComponent) },


];
