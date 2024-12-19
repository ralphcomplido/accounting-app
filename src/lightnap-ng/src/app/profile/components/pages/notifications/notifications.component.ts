import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ApiResponseComponent, ToastService } from "@core";
import { ErrorListComponent } from "@core";
import { ProfileService, Notification } from "@profile";
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
  readonly #profileService = inject(ProfileService);
  readonly #toast = inject(ToastService);

  notifications$: Observable<Array<Notification>>;

  errors = new Array<string>();

  constructor() {
    this.#refresh();
  }

  #refresh() {
    this.notifications$ = this.#profileService.searchNotifications({}).pipe(map(results => results.data));
  }

  markAsRead(id: number) {
    this.#profileService.markNotificationAsRead(id).subscribe({
      next: () => this.#refresh(),
      error: response => (this.errors = response.errorMessages),
    });
  }

  markAllAsRead() {
    this.#profileService.markAllNotificationsAsRead().subscribe({
      next: () => {
        this.#toast.success("All notifications marked as read.");
        this.#refresh();
      },
      error: response => (this.errors = response.errorMessages),
    });
  }
}
