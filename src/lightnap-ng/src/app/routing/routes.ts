import { adminGuard } from "@identity/guards/admin.guard";
import { authGuard } from "@identity/guards/auth.guard";
import { PublicLayoutComponent } from "@layout/components/layouts/public-layout/public-layout.component";
import { Routes as AdminRoutes } from "../admin/components/pages/routes";
import { Routes as IdentityRoutes } from "../identity/components/pages/routes";
import { AppLayoutComponent } from "../layout/components/layouts/app-layout/app-layout.component";
import { Routes as ProfileRoutes } from "../profile/components/pages/routes";
import { Routes as PublicRoutes } from "../public/components/pages/routes";
import { Routes as UserRoutes } from "../user/components/pages/routes";
import { AppRoute } from "./models/app-route";


export const Routes: AppRoute[] = [
  { path: "", component: PublicLayoutComponent, children: PublicRoutes },
  {
    path: "",
    component: AppLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: "home", data: { breadcrumb: "Home" }, children: UserRoutes },
      { path: "profile", data: { breadcrumb: "Profile" }, children: ProfileRoutes }
      
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
