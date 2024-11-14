import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { BlockUiService, ErrorListComponent, ToastService } from "@core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { ProfileService } from "@profile/services/profile.service";
import { RouteAliasService } from "@routing";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { finalize, tap } from "rxjs";
import { IdentityService } from "src/app/identity/services/identity.service";

@Component({
  standalone: true,
  templateUrl: "./index.component.html",
  imports: [CommonModule, ErrorListComponent, ReactiveFormsModule, ButtonModule, CardModule, ApiResponseComponent],
})
export class IndexComponent {
  #identityService = inject(IdentityService);
  #profileService = inject(ProfileService);
  #routeAlias = inject(RouteAliasService);
  #blockUi = inject(BlockUiService);
  #toast = inject(ToastService);
  #fb = inject(FormBuilder);

  form = this.#fb.group({});
  errors = new Array<string>();

  profile$ = this.#profileService.getProfile().pipe(
    tap(profile => {
      // Set form values.
    })
  );

  updateProfile() {
    this.#blockUi.show({ message: "Updating profile..." });
    this.#profileService
      .updateProfile({})
      .pipe(finalize(() => this.#blockUi.hide()))
      .subscribe({
        next: () => this.#toast.success("Profile updated successfully."),
        error: response => (this.errors = response.errorMessages),
      });
  }

  logOut() {
    this.#blockUi.show({ message: "Logging out..." });
    this.#identityService
      .logOut()
      .pipe(finalize(() => this.#blockUi.hide()))
      .subscribe({
        next: () => this.#routeAlias.navigate("landing"),
        error: response => (this.errors = response.errorMessages),
      });
  }
}
