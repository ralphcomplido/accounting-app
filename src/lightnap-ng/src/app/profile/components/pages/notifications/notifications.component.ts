import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { Router } from "@angular/router";
import { ApiResponseComponent, ErrorListComponent, ToastService } from "@core";
import { NotificationData, NotificationService } from "@profile";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { TableModule } from "primeng/table";
import { map, Observable } from "rxjs";

@Component({
  standalone: true,
  templateUrl: "./notifications.component.html",
  imports: [CommonModule, TableModule, ButtonModule, ErrorListComponent, CardModule, ApiResponseComponent],
})
export class NotificationsComponent {
  readonly #notificationService = inject(NotificationService);
  readonly #toast = inject(ToastService);
  readonly #router = inject(Router);

  notifications$: Observable<Array<NotificationData>>;

  errors = new Array<string>();

  constructor() {
    this.#refresh();
  }

  #refresh() {
    this.notifications$ = this.#notificationService.searchNotifications({}).pipe(map(results => results.data));
  }

  notificationClicked(notification: NotificationData) {
    this.#notificationService.markNotificationAsRead(notification.id).subscribe();
    this.#router.navigate(notification.routerLink);
  }

  markAllAsRead() {
    this.#notificationService.markAllNotificationsAsRead().subscribe({
      next: () => {
        this.#toast.success("All notifications marked as read.");
        this.#refresh();
      },
      error: response => (this.errors = response.errorMessages),
    });
  }
}
