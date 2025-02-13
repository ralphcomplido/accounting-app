import { AdminUser, Role } from "@admin/models";
import { AdminService } from "@admin/services/admin.service";
import { CommonModule } from "@angular/common";
import { Component, inject, input, OnChanges } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { ConfirmPopupComponent, ToastService } from "@core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { RouteAliasService } from "@routing";
import { ConfirmationService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { PanelModule } from "primeng/panel";
import { TabsModule } from "primeng/tabs";
import { TagModule } from "primeng/tag";
import { Observable } from "rxjs";
import { UserProfileComponent } from "./user-profile/user-profile.component";
import { UserRolesComponent } from "./user-roles/user-roles.component";

@Component({
  standalone: true,
  templateUrl: "./user.component.html",
  imports: [
    CommonModule,
    ReactiveFormsModule,
    PanelModule,
    ButtonModule,
    ErrorListComponent,
    ApiResponseComponent,
    ConfirmPopupComponent,
    TagModule,
    TabsModule,
    UserRolesComponent,
    UserProfileComponent,
  ],
})
export class UserComponent implements OnChanges {
  adminService = inject(AdminService);
  #confirmationService = inject(ConfirmationService);
  #toast = inject(ToastService);
  #routeAlias = inject(RouteAliasService);
  #fb = inject(FormBuilder);

  userId = input.required<string>();

  errors: string[] = [];

  addUserToRoleForm = this.#fb.group({
    role: this.#fb.control("", [Validators.required]),
  });

  user$ = new Observable<AdminUser>();
  userRoles$ = new Observable<Array<Role>>();

  ngOnChanges() {
    this.#refreshUser();
    this.#refreshRoles();
  }

  #refreshUser() {
    this.user$ = this.adminService.getUser(this.userId());
  }

  #refreshRoles() {
    this.userRoles$ = this.adminService.getUserRoles(this.userId());
  }

  lockUserAccount(event: any) {
    this.errors = [];

    this.#confirmationService.confirm({
      header: "Confirm Lock Account",
      message: `Are you sure that you want to lock this user account?`,
      target: event.target,
      key: "lock",
      accept: () => {
        this.adminService.lockUserAccount(this.userId()).subscribe({
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
        this.adminService.unlockUserAccount(this.userId()).subscribe({
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
        this.adminService.deleteUser(this.userId()).subscribe({
          next: () => {
            this.#toast.success("User deleted successfully.");
            this.#routeAlias.navigate("admin-users");
          },
          error: response => (this.errors = response.errorMessages),
        });
      },
    });
  }

  removeRole(role: string) {
    this.errors = [];

    this.adminService.removeUserFromRole(this.userId(), role).subscribe({
      next: () => this.#refreshRoles(),
      error: response => (this.errors = response.errorMessages),
    });
  }

  addRole(role: string) {
    this.errors = [];

    this.adminService.addUserToRole(this.userId(), role).subscribe({
      next: () => this.#refreshRoles(),
      error: response => (this.errors = response.errorMessages),
    });
  }
}
