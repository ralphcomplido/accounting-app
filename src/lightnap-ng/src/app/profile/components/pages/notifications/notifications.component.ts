import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { Router } from "@angular/router";
import { ApiResponseComponent, EmptyPagedResponse, ErrorListComponent, ToastService } from "@core";
import { NotificationData, NotificationService } from "@profile";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { TableLazyLoadEvent, TableModule } from "primeng/table";
import { debounceTime, map, Observable, startWith, Subject, switchMap } from "rxjs";

@Component({
  standalone: true,
  templateUrl: "./notifications.component.html",
  imports: [CommonModule, TableModule, ButtonModule, ErrorListComponent, CardModule, ApiResponseComponent],
})
export class NotificationsComponent {
  readonly pageSize = 10;

  readonly #notificationService = inject(NotificationService);
  readonly #toast = inject(ToastService);
  readonly #router = inject(Router);

  #lazyLoadEventSubject = new Subject<TableLazyLoadEvent>();
  notifications$ = this.#lazyLoadEventSubject.pipe(
    switchMap(event =>
      this.#notificationService.searchNotifications({
        pageSize: this.pageSize,
        pageNumber: event.first / this.pageSize + 1,
      })
    ),
    // We need to bootstrap the p-table with a response to get the whole process running. We do it this way to
    // fake an empty response so we can avoid a redundant call to the API.
    startWith(new EmptyPagedResponse<NotificationData>())
  );

  errors = new Array<string>();

  onLazyLoad(event: TableLazyLoadEvent) {
    this.#lazyLoadEventSubject.next(event);
  }

  notificationClicked(notification: NotificationData) {
    this.#notificationService.markNotificationAsRead(notification.id).subscribe();
    this.#router.navigate(notification.routerLink);
  }

  markAllAsRead() {
    this.#notificationService.markAllNotificationsAsRead().subscribe({
      next: () => {
        this.#toast.success("All notifications marked as read.");
        this.#lazyLoadEventSubject.next({ first: 0 });
      },
      error: response => (this.errors = response.errorMessages),
    });
  }
}
