import { AppRoute } from "@routing";

export const Routes: AppRoute[] = [
  { path: "", data: { alias: "profile" }, loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "devices", data: { alias: "devices" }, loadComponent: () => import("./devices/devices.component").then(m => m.DevicesComponent) },
  {
    path: "change-password",
    data: { alias: "change-password" },
    loadComponent: () => import("./change-password/change-password.component").then(m => m.ChangePasswordComponent),
  },
  {
    path: "change-email",
    data: { alias: "change-email" },
    loadComponent: () => import("./change-email/change-email.component").then(m => m.ChangeEmailComponent),
  },
  {
    path: "change-email-requested",
    data: { alias: "change-email-requested" },
    loadComponent: () => import("./change-email-requested/change-email-requested.component").then(m => m.ChangeEmailRequestedComponent),
  },
  {
    path: "confirm-email-change/:newEmail/:code",
    loadComponent: () => import("./confirm-email-change/confirm-email-change.component").then(m => m.ConfirmEmailChangeComponent),
  },
];
