import { Component, inject } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterLink } from "@angular/router";
import { PanelModule } from "primeng/panel";
import { TableModule } from "primeng/table";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";

@Component({
  standalone: true,
  templateUrl: "./account.component.html",
  imports: [
    CommonModule
  ]
})
export class AccountComponent {
  // readonly #accountService = inject(AccountService);

  // readonly accounts$ = this.#accountService.getAccounts();
}
