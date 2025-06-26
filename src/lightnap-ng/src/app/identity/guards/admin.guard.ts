import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, createUrlTreeFromSnapshot } from "@angular/router";
import { IdentityService } from "src/app/identity/services/identity.service";
import { map, take } from "rxjs";
import { RouteAliasService } from "@routing";

export const adminGuard = (next: ActivatedRouteSnapshot) => {
  const identityService = inject(IdentityService);
  const routeAliasService = inject(RouteAliasService);

  return identityService
    .watchUserRole$("Administrator")
    .pipe(take(1), map(isAdmin => (isAdmin ? true : createUrlTreeFromSnapshot(next, routeAliasService.getRoute("access-denied")))));
};
