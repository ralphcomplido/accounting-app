import { CommonModule } from "@angular/common";
import { Component, inject, input, output } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ConfirmPopupComponent } from "@core";
import { Claim } from "@identity";
import { RoutePipe } from "@routing";
import { ConfirmationService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { InputTextModule } from "primeng/inputtext";
import { SelectModule } from "primeng/select";
import { TableModule } from "primeng/table";

@Component({
  standalone: true,
  selector: "user-claims",
  templateUrl: "./user-claims.component.html",
  imports: [CommonModule, ReactiveFormsModule, InputTextModule, TableModule, ButtonModule, SelectModule, ConfirmPopupComponent, RoutePipe, RouterModule],
})
export class UserClaimsComponent {
  #confirmationService = inject(ConfirmationService);
  #fb = inject(FormBuilder);

  addUserClaimForm = this.#fb.group({
    type: this.#fb.control("", [Validators.required]),
    value: this.#fb.control("", [Validators.required]),
  });

  userClaims = input.required<Array<Claim>>();
  addClaim = output<Claim>();
  removeClaim = output<Claim>();

  removeClaimClicked(event: any, claim: Claim) {
    this.#confirmationService.confirm({
      header: "Confirm Claim Removal",
      message: `Are you sure that you want to remove this claim?`,
      target: event.target,
      key: claim.type + ":" + claim.value,
      accept: () => this.removeClaim.emit(claim),
    });
  }

  addClaimClicked() {
    if (!this.addUserClaimForm.valid) return;

    this.addClaim.emit(this.addUserClaimForm.value as Claim);
    this.addUserClaimForm.reset();
  }
}
