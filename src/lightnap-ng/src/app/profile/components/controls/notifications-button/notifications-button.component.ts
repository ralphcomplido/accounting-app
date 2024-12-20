import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { Router } from "@angular/router";
import { NotificationService } from "@profile/services";
import { RouteAliasService } from "@routing";
import { MenuItem } from "primeng/api";
import { BadgeModule } from "primeng/badge";
import { ButtonModule } from "primeng/button";
import { MenuModule } from "primeng/menu";
import { take, tap } from "rxjs";

@Component({
  selector: "notifications-button",
  standalone: true,
  templateUrl: "./notifications-button.component.html",
  imports: [CommonModule, ButtonModule, MenuModule, BadgeModule],
})
export class NotificationsButtonComponent {
  readonly #notificationService = inject(NotificationService);
  readonly #routeAlias = inject(RouteAliasService);
  readonly #router = inject(Router);
  readonly latestNotifications$ = this.#notificationService.watchLatest$().pipe(
    tap(
      latest =>
        (this.items = [
          ...latest.notifications.map(
            n =>
              <MenuItem>{
                label: n.title,
                command: () => {
                    this.#notificationService.markNotificationAsRead(n.id).subscribe();
                    this.#router.navigate(n.routerLink);
                }
              }
          ),
          ...this.endMenuItems,
        ])
    )
  );
  readonly endMenuItems = new Array<MenuItem>(
    {
      label: "Mark all as read",
      icon: "pi pi-check",
      command: () => this.markAllAsRead(),
    },
    {
      label: "See all notifications",
      icon: "pi pi-check",
      command: () => this.#routeAlias.navigate("notifications"),
    }
  );

  items = new Array<MenuItem>(...this.endMenuItems);

  markAllAsRead() {
    this.#notificationService.markAllNotificationsAsRead().subscribe();
  }
}
