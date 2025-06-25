import { AdminService } from "@admin/services/admin.service";
import { CommonModule } from "@angular/common";
import { Component, inject, input, output } from "@angular/core";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { ConfirmPopupComponent } from "@core";
import { Claim } from "@identity";
import { ConfirmationService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { SelectModule } from "primeng/select";
import { TableModule } from "primeng/table";

@Component({
  standalone: true,
  selector: "user-claims",
  templateUrl: "./user-claims.component.html",
  imports: [CommonModule, ReactiveFormsModule, TableModule, ButtonModule, SelectModule, ConfirmPopupComponent],
})
export class UserClaimsComponent {
  #adminService = inject(AdminService);
  #confirmationService = inject(ConfirmationService);
  #fb = inject(FormBuilder);

  userClaims = input.required<Array<Claim>>();
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
}
