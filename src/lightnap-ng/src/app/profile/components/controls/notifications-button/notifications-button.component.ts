import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ProfileService } from "@profile/services/profile.service";
import { RouteAliasService, RoutePipe } from "@routing";
import { MenuItem } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { BadgeModule } from "primeng/badge";
import { MenuModule } from "primeng/menu";
import { take } from "rxjs";

@Component({
  selector: "notifications-button",
  standalone: true,
  templateUrl: "./notifications-button.component.html",
  imports: [CommonModule, ButtonModule, MenuModule, BadgeModule, RoutePipe],
})
export class NotificationsButtonComponent {
  readonly #profileService = inject(ProfileService);
  readonly #routeAlias = inject(RouteAliasService);
  readonly notifications$ = this.#profileService.watchUnreadNotifications$();
  readonly items = new Array<MenuItem>(
    {
      label: "Mark all as read",
      icon: "pi pi-check",
      command: () => this.markAllAsRead(),
    },
    {
      label: "Mark first as read",
      icon: "pi pi-check",
      command: () => this.markFirstAsRead(),
    },
    {
        label: "See all notifications",
        icon: "pi pi-check",
        command: () => this.#routeAlias.navigate("notifications"),
      }

  );

  markFirstAsRead() {
    this.#profileService
      .watchUnreadNotifications$()
      .pipe(take(1))
      .subscribe({
        next: notifications => {
          if (notifications.length > 0) {
            this.#profileService.markNotificationAsRead(notifications[0].id).subscribe();
          }
        },
      });
  }

  markAllAsRead() {
    this.#profileService.markAllNotificationsAsRead().subscribe();
  }
}
