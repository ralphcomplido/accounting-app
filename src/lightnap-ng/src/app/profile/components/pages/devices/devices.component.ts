import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ConfirmDialogComponent } from "@core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { ProfileService } from "@profile/services/profile.service";
import { ConfirmationService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { TableModule } from "primeng/table";

@Component({
  standalone: true,
  templateUrl: "./devices.component.html",
  imports: [CommonModule, TableModule, ButtonModule, ErrorListComponent, CardModule, ApiResponseComponent, ConfirmDialogComponent],
})
export class DevicesComponent {
  #profileService = inject(ProfileService);
  #confirmationService = inject(ConfirmationService);

  devices$ = this.#profileService.getDevices();

  errors = new Array<string>();

  revokeDevice(event: any, deviceId: string) {
    this.#confirmationService.confirm({
      header: "Confirm Revoke",
      message: `Are you sure that you want to revoke this device?`,
      target: event.target,
      key: deviceId,
      accept: () => {
        this.#profileService
          .revokeDevice(deviceId)
          .subscribe({
            next: () => (this.devices$ = this.#profileService.getDevices()),
            error: response => (this.errors = response.errorMessages),
          });
      },
    });
  }
}
