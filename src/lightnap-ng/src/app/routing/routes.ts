import { ExtraOptions } from "@angular/router";
import { adminGuard } from "@identity/guards/admin.guard";
import { authGuard } from "@identity/guards/auth.guard";
import { Routes as AdminRoutes } from "../admin/components/pages/routes";
import { Routes as IdentityRoutes } from "../identity/components/pages/routes";
import { AppLayoutComponent } from "../layout/components/layouts/app-layout/app-layout.component";
import { Routes as ProfileRoutes } from "../profile/components/pages/routes";
import { Routes as PublicRoutes } from "../public/components/pages/routes";
import { Routes as UserRoutes } from "../user/components/pages/routes";
import { AppRoute } from "./models/app-route";

export const Routes: AppRoute[] = [
  { path: "", children: PublicRoutes },
  {
    path: "",
    component: AppLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: "home", data: { breadcrumb: "Home" }, children: UserRoutes },
      { path: "profile", data: { breadcrumb: "Profile" }, children: ProfileRoutes },
    ],
  },
  {
    path: "admin",
    component: AppLayoutComponent,
    canActivate: [authGuard, adminGuard],
    children: [{ path: "", data: { breadcrumb: "Admin" }, children: AdminRoutes }],
  },
  { path: "identity", data: { breadcrumb: "Identity" }, children: IdentityRoutes },
  { path: "**", redirectTo: "/not-found" },
];

export const RouteConfig: ExtraOptions = {
  bindToComponentInputs: true,
  scrollPositionRestoration: "enabled",
  anchorScrolling: "enabled",
  onSameUrlNavigation: "reload",
};
