import { AdminUserWithRoles } from "@admin/models";
import { AdminService } from "@admin/services/admin.service";
import { CommonModule } from "@angular/common";
import { Component, inject, input, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterLink } from "@angular/router";
import { ConfirmPopupComponent, ToastService } from "@core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { RouteAliasService, RoutePipe } from "@routing";
import { ConfirmationService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { DropdownModule } from "primeng/dropdown";
import { TableModule } from "primeng/table";
import { TagModule } from "primeng/tag";
import { Observable } from "rxjs";

@Component({
  standalone: true,
  templateUrl: "./user.component.html",
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    TableModule,
    ButtonModule,
    RouterLink,
    RoutePipe,
    ErrorListComponent,
    ApiResponseComponent,
    DropdownModule,
    ConfirmPopupComponent,
    TagModule,
  ],
})
export class UserComponent implements OnInit {
  #adminService = inject(AdminService);
  #confirmationService = inject(ConfirmationService);
  #toast = inject(ToastService);
  #routeAlias = inject(RouteAliasService);
  #fb = inject(FormBuilder);

  userId = input.required<string>();

  errors: string[] = [];

  addUserToRoleForm = this.#fb.group({
    role: this.#fb.control("", [Validators.required]),
  });

  userWithRoles$ = new Observable<AdminUserWithRoles>();

  roles$ = this.#adminService.getRoles();

  ngOnInit() {
    this.#refreshUser();
  }

  #refreshUser() {
    this.userWithRoles$ = this.#adminService.getUserWithRoles(this.userId());
  }

  removeUserFromRole(event: any, role: string) {
    this.errors = [];

    this.#confirmationService.confirm({
      header: "Confirm Role Removal",
      message: `Are you sure that you want to remove this role membership?`,
      target: event.target,
      key: role,
      accept: () => {
        this.#adminService
          .removeUserFromRole(this.userId(), role)
          .subscribe({
            next: () => this.#refreshUser(),
            error: response => (this.errors = response.errorMessages),
          });
      },
    });
  }

  addUserToRole() {
    this.errors = [];

    this.#adminService
      .addUserToRole(this.userId(), this.addUserToRoleForm.value.role)
      .subscribe({
        next: () => this.#refreshUser(),
        error: response => (this.errors = response.errorMessages),
      });
  }

  lockUserAccount(event: any) {
    this.errors = [];

    this.#confirmationService.confirm({
      header: "Confirm Lock Account",
      message: `Are you sure that you want to lock this user account?`,
      target: event.target,
      key: "lock",
      accept: () => {
        this.#adminService
          .lockUserAccount(this.userId())
          .subscribe({
            next: () => this.#refreshUser(),
            error: response => (this.errors = response.errorMessages),
          });
      },
    });
  }

  unlockUserAccount(event: any) {
    this.errors = [];

    this.#confirmationService.confirm({
      header: "Confirm Unlock Account",
      message: `Are you sure that you want to unlock this user account?`,
      target: event.target,
      key: "unlock",
      accept: () => {
        this.#adminService.unlockUserAccount(this.userId()).subscribe({
          next: () => this.#refreshUser(),
          error: response => (this.errors = response.errorMessages),
        });
      },
    });
  }

  deleteUser(event: any) {
    this.errors = [];

    this.#confirmationService.confirm({
      header: "Confirm Delete User",
      message: `Are you sure that you want to delete this user?`,
      target: event.target,
      key: "delete",
      accept: () => {
        this.#adminService.deleteUser(this.userId()).subscribe({
          next: () => {
            this.#toast.success("User deleted successfully.");
            this.#routeAlias.navigate("admin-users");
          },
          error: response => (this.errors = response.errorMessages),
        });
      },
    });
  }
}
