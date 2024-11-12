import { AdminUser, AdminUserWithRoles, Role, SearchAdminUsersRequest, UpdateAdminUserRequest } from "@admin/models";
import { inject, Injectable } from "@angular/core";
import { ApiResponse, catchApiError, SuccessApiResponse, throwIfApiError } from "@core";
import { forkJoin, map, Observable, of, tap } from "rxjs";
import { DataService } from "./data.service";

/**
 * Service for Administrator management of users, roles, and other data.
 */
@Injectable({
  providedIn: "root",
})
export class AdminService {
  #dataService = inject(DataService);
  #rolesResponse?: ApiResponse<Array<Role>>;

  /**
   * Gets a user by their ID.
   * @param {string} userId - The ID of the user to retrieve.
   * @returns {Observable<ApiResponse<AdminUser>>} An observable containing the user data.
   */
  getUser(userId: string) {
    return this.#dataService.getUser(userId);
  }

  /**
   * Updates a user by their ID.
   * @param {string} userId - The ID of the user to update.
   * @param {UpdateAdminUserRequest} updateAdminUser - The update request object.
   * @returns {Observable<ApiResponse<AdminUser>>} An observable with the updated user.
   */
  updateUser(userId: string, updateAdminUser: UpdateAdminUserRequest) {
    return this.#dataService.updateUser(userId, updateAdminUser);
  }

  /**
   * Deletes a user by their ID.
   * @param {string} userId - The ID of the user to delete.
   * @returns {Observable<ApiResponse<boolean>>} An observable indicating the deletion result.
   */
  deleteUser(userId: string) {
    return this.#dataService.deleteUser(userId);
  }

  /**
   * Searches for users based on the search criteria.
   * @param {SearchAdminUsersRequest} searchAdminUsers - The search criteria.
   * @returns {Observable<ApiResponse<Array<AdminUser>>>} An observable containing the search results.
   */
  searchUsers(searchAdminUsers: SearchAdminUsersRequest) {
    return this.#dataService.searchUsers(searchAdminUsers);
  }

  /**
   * Gets the list of roles.
   * @returns {Observable<ApiResponse<Array<Role>>>} An observable containing the roles.
   */
  getRoles(): Observable<ApiResponse<Array<Role>>> {
    if (this.#rolesResponse) return of(this.#rolesResponse);
    return this.#dataService.getRoles().pipe(
      throwIfApiError(),
      tap(response => (this.#rolesResponse = response)),
      catchApiError()
    );
  }

  /**
   * Gets a role by its name.
   * @param {string} roleName - The name of the role to retrieve.
   * @returns {Observable<ApiResponse<Role>>} An observable containing the role data.
   */
  getRole(roleName: string) {
    return this.getRoles().pipe(
      throwIfApiError(),
      map(response => new SuccessApiResponse(response.result.find(role => role.name === roleName))),
      catchApiError()
    );
  }

  /**
   * Gets the roles a user belongs to.
   * @param {string} userId - The user.
   * @returns {Observable<ApiResponse<Array<Role>>>} An observable containing the roles.
   */
  getUserRoles(userId: string) {
    return forkJoin([this.getRoles(), this.#dataService.getUserRoles(userId)]).pipe(
      map(([rolesResponse, userRolesResponse]) => {
        if (!rolesResponse.result) return rolesResponse as any as ApiResponse<Array<Role>>;
        if (!userRolesResponse.result) return userRolesResponse as any as ApiResponse<Array<Role>>;
        return new SuccessApiResponse(userRolesResponse.result.map(userRole => rolesResponse.result.find(role => role.name === userRole)));
      })
    );
  }

  /**
   * Gets users in the specified role.
   * @param {string} role - The role.
   * @returns {Observable<ApiResponse<Array<AdminUser>>>} An observable containing the members.
   */
  getUsersInRole(role: string) {
    return this.#dataService.getUsersInRole(role);
  }

  /**
   * Adds a user to a role.
   * @param {string} userId - The user to add to the role.
   * @param {string} role - The role.
   * @returns {Observable<ApiResponse<boolean>>} An observable with a result of true if successful.
   */
  addUserToRole(userId: string, role: string) {
    return this.#dataService.addUserToRole(userId, role);
  }

  /**
   * Removes a user from a role.
   * @param {string} userId - The user to remove from the role.
   * @param {string} role - The role.
   * @returns {Observable<ApiResponse<Role>>} An observable with a result of true if successful.
   */
  removeUserFromRole(userId: string, role: string) {
    return this.#dataService.removeUserFromRole(userId, role);
  }

  /**
   * Locks a user account.
   * @param {string} userId - The user to lock.
   * @returns {Observable<ApiResponse<boolean>>} An observable with a result of true if successful.
   */
  lockUserAccount(userId: string) {
    return this.#dataService.lockUserAccount(userId);
  }

  /**
   * Unlocks a user account.
   * @param {string} userId - The user to lock.
   * @returns {Observable<ApiResponse<boolean>>} An observable with a result of true if successful.
   */
  unlockUserAccount(userId: string) {
    return this.#dataService.unlockUserAccount(userId);
  }

  /**
   * Gets a user with their roles.
   * @param {string} userId - The user.
   * @returns {Observable<ApiResponse<AdminUserWithRoles>>} An observable containing the user and roles.
   */
  getUserWithRoles(userId: string) {
    return forkJoin([this.getUser(userId), this.getUserRoles(userId)]).pipe(
      map(([userResponse, rolesResponse]) => {
        if (!userResponse.result) return userResponse as any as ApiResponse<AdminUserWithRoles>;
        if (!rolesResponse.result) return rolesResponse as any as ApiResponse<AdminUserWithRoles>;

        return new SuccessApiResponse<AdminUserWithRoles>({
          user: userResponse.result,
          roles: rolesResponse.result,
        });
      })
    );
  }
}
