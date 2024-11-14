import { Component, Input, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { BlockUiService, ErrorListComponent } from "@core";
import { RouteAliasService, RoutePipe } from "@routing";
import { ButtonModule } from "primeng/button";
import { CheckboxModule } from "primeng/checkbox";
import { InputTextModule } from "primeng/inputtext";
import { finalize } from "rxjs";
import { IdentityService } from "src/app/identity/services/identity.service";
import { FocusContentLayout } from "src/app/layout/components/layouts/focus-content-layout/focus-content-layout.component";
import { LayoutService } from "src/app/layout/services/layout.service";

@Component({
  standalone: true,
  templateUrl: "./verify-code.component.html",
  imports: [
    ReactiveFormsModule,
    RouterModule,
    ButtonModule,
    InputTextModule,
    CheckboxModule,
    RoutePipe,
    FocusContentLayout,
    ErrorListComponent,
  ],
})
export class VerifyCodeComponent {
  #identityService = inject(IdentityService);
  #blockUi = inject(BlockUiService);
  #fb = inject(FormBuilder);
  #routeAlias = inject(RouteAliasService);
  layoutService = inject(LayoutService);

  @Input() email = "";

  form = this.#fb.group({
    code1: this.#fb.control("", [Validators.required]),
    code2: this.#fb.control("", [Validators.required]),
    code3: this.#fb.control("", [Validators.required]),
    code4: this.#fb.control("", [Validators.required]),
    code5: this.#fb.control("", [Validators.required]),
    code6: this.#fb.control("", [Validators.required]),
    paste: this.#fb.control(""),
    rememberMe: this.#fb.control(false),
  });

  errors: Array<string> = [];

  constructor() {
    this.form.controls.paste.valueChanges.subscribe({
      next: value => {
        if (!value) {
          return;
        }

        value = value.trim();
        this.form.controls.paste.setValue("");

        this.form.controls.code1.setValue(value.length >= 1 ? value[0] : "");
        this.form.controls.code2.setValue(value.length >= 2 ? value[1] : "");
        this.form.controls.code3.setValue(value.length >= 3 ? value[2] : "");
        this.form.controls.code4.setValue(value.length >= 4 ? value[3] : "");
        this.form.controls.code5.setValue(value.length >= 5 ? value[4] : "");
        this.form.controls.code6.setValue(value.length >= 6 ? value[5] : "");
        if (value.length == 6) {
          this.onVerifyClicked();
        }
      },
    });
  }

  onDigitInput(event: any) {
    let element;
    if (event.code !== "Backspace") {
      //if (event.code.includes("Numpad") || event.code.includes("Digit")) {
      if (!isNaN(event.key)) {
        element = event.srcElement.nextElementSibling;
        if (!element) {
          event.srcElement.blur();
          this.onVerifyClicked();
        }
      }
    } else {
      element = event.srcElement.previousElementSibling;
    }

    if (element) {
      element.focus();
    }
  }

  onVerifyClicked() {
    const value = this.form.value;
    const code = `${value.code1}${value.code2}${value.code3}${value.code4}${value.code5}${value.code6}`;

    this.#blockUi.show({ message: "Verifying code..." });
    this.#identityService
      .verifyCode({
        code,
        email: this.email,
        deviceDetails: navigator.userAgent,
        rememberMe: value.rememberMe,
      })
      .pipe(finalize(() => this.#blockUi.hide()))
      .subscribe({
        next: () => this.#routeAlias.navigate("user-home"),
        error: response => (this.errors = response.errorMessages),
      });
  }
}
