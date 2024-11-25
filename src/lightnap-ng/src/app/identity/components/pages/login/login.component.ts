import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { BlockUiService, ErrorListComponent } from "@core";
import { IdentityCardComponent } from "@identity/components/controls/identity-card/identity-card.component";
import { RouteAliasService, RoutePipe } from "@routing";
import { ButtonModule } from "primeng/button";
import { CheckboxModule } from "primeng/checkbox";
import { InputTextModule } from "primeng/inputtext";
import { PasswordModule } from "primeng/password";
import { finalize } from "rxjs";
import { IdentityService } from "src/app/identity/services/identity.service";
import { LayoutService } from "src/app/layout/services/layout.service";

@Component({
  standalone: true,
  templateUrl: "./login.component.html",
  imports: [
    ReactiveFormsModule,
    RouterModule,
    ButtonModule,
    InputTextModule,
    CheckboxModule,
    RoutePipe,
    PasswordModule,
    IdentityCardComponent,
    ErrorListComponent,
  ],
})
export class LoginComponent {
  layoutService = inject(LayoutService);
  #identityService = inject(IdentityService);
  #router = inject(Router);
  #blockUi = inject(BlockUiService);
  #fb = inject(FormBuilder);
  #routeAlias = inject(RouteAliasService);

  form = this.#fb.nonNullable.group({
    email: this.#fb.control("", [Validators.required, Validators.email]),
    password: this.#fb.control("", [Validators.required]),
    rememberMe: this.#fb.control(true),
  });

  errors: Array<string> = [];

  logIn() {
    this.#blockUi.show({ message: "Logging in..." });

    this.#identityService
      .logIn({
        email: this.form.value.email,
        password: this.form.value.password,
        rememberMe: this.form.value.rememberMe,
        deviceDetails: navigator.userAgent,
      })
      .pipe(finalize(() => this.#blockUi.hide()))
      .subscribe({
        next: result => {
          if (result.twoFactorRequired) {
            this.#routeAlias.navigate("verify-code", this.form.value.email);
          } else {
            const redirectUrl = this.#identityService.redirectUrl;
            if (redirectUrl) {
              this.#router.navigateByUrl(redirectUrl);
            } else {
              this.#routeAlias.navigate("user-home");
            }
          }
        },
        error: response => (this.errors = response.errorMessages),
      });
  }
}
