import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ProfileService } from "@profile/services/profile.service";
import { BadgeModule } from "primeng/badge";
import { ButtonModule } from "primeng/button";
import { take } from "rxjs";

@Component({
  selector: "notifications-button",
  standalone: true,
  templateUrl: "./notifications-button.component.html",
  imports: [CommonModule, ButtonModule, BadgeModule],
})
export class NotificationsButtonComponent {
  readonly #profileService = inject(ProfileService);
  readonly notifications$ = this.#profileService.watchUnreadNotifications$();

  markFirstAsRead() {
    this.#profileService.markAllNotificationsAsRead().subscribe();
  }
}
