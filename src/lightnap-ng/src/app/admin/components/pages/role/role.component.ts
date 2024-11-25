import { AdminService } from "@admin/services/admin.service";
import { CommonModule } from "@angular/common";
import { Component, inject, input, OnInit } from "@angular/core";
import { RouterLink } from "@angular/router";
import { ConfirmPopupComponent } from "@core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { RoutePipe } from "@routing";
import { ConfirmationService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { TableModule } from "primeng/table";
import { Observable, tap } from "rxjs";
import { RoleViewModel } from "./role-view-model";

@Component({
  standalone: true,
  templateUrl: "./role.component.html",
  imports: [
    CommonModule,
    CardModule,
    TableModule,
    ButtonModule,
    RouterLink,
    RoutePipe,
    ErrorListComponent,
    ApiResponseComponent,
    ConfirmPopupComponent,
  ],
})
export class RoleComponent implements OnInit {
  #adminService = inject(AdminService);
  #confirmationService = inject(ConfirmationService);

  role = input.required<string>();

  header = "Loading role...";
  subHeader = "";
  errors: string[] = [];

  roleWithUsers$ = new Observable<RoleViewModel>();

  ngOnInit() {
    this.#refreshRole();
  }

  #refreshRole() {
    this.roleWithUsers$ = this.#adminService.getRoleWithUsers(this.role()).pipe(
      tap(roleWithAdminUsers => {
        this.header = `Manage Users In Role: ${roleWithAdminUsers.role.displayName}`;
        this.subHeader = roleWithAdminUsers.role.description;
      })
    );
  }

  removeUserFromRole(event: any, userId: string) {
    this.errors = [];

    this.#confirmationService.confirm({
      header: "Confirm Role Removal",
      message: `Are you sure that you want to remove this role membership?`,
      target: event.target,
      key: userId,
      accept: () => {
        this.#adminService.removeUserFromRole(userId, this.role()).subscribe({
          next: () => this.#refreshRole(),
          error: response => (this.errors = response.errorMessages),
        });
      },
    });
  }
}
