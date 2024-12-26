import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { NotificationService } from "@profile/services";
import { RouteAliasService, RoutePipe } from "@routing";
import { MenuItem } from "primeng/api";
import { BadgeModule } from "primeng/badge";
import { ButtonModule } from "primeng/button";
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { tap } from "rxjs";
import { NotificationItemComponent } from "../notification-item/notification-item.component";

@Component({
  selector: "notifications-button",
  standalone: true,
  templateUrl: "./notifications-button.component.html",
  imports: [CommonModule, ButtonModule, OverlayPanelModule, BadgeModule, NotificationItemComponent, RoutePipe, RouterLink],
})
export class NotificationsButtonComponent {
  readonly #notificationService = inject(NotificationService);
  readonly #routeAlias = inject(RouteAliasService);
  readonly #router = inject(Router);
  readonly latestNotifications$ = this.#notificationService.watchLatest$();
  readonly endMenuItems = new Array<MenuItem>(
    {
      label: "Mark all as read",
      icon: "pi pi-folder-open",
      command: () => this.markAllAsRead(),
    },
    {
      label: "See all",
      icon: "pi pi-images",
      command: () => this.#routeAlias.navigate("notifications"),
    }
  );

  items = new Array<MenuItem>(...this.endMenuItems);

  markAllAsRead() {
    this.#notificationService.markAllNotificationsAsRead().subscribe();
  }
}
