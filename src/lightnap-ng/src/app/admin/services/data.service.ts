import { AdminUser, Role, SearchAdminUsersRequest, UpdateAdminUserRequest } from "@admin/models";
import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT, ApiResponse, PagedResponse } from "@core";

@Injectable({
  providedIn: "root",
})
export class DataService {
  #http = inject(HttpClient);
  #apiUrlRoot = `${inject(API_URL_ROOT)}administrator/`;

  getUser(userId: string) {
    return this.#http.get<AdminUser>(`${this.#apiUrlRoot}users/${userId}`);
  }

  updateUser(userId: string, updateAdminUser: UpdateAdminUserRequest) {
    return this.#http.put<AdminUser>(`${this.#apiUrlRoot}users/${userId}`, updateAdminUser);
  }

  deleteUser(userId: string) {
    return this.#http.delete<boolean>(`${this.#apiUrlRoot}users/${userId}`);
  }

  searchUsers(searchAdminUsers: SearchAdminUsersRequest) {
    return this.#http.post<PagedResponse<AdminUser>>(`${this.#apiUrlRoot}users/search`, searchAdminUsers);
  }

  getRoles() {
    return this.#http.get<Array<Role>>(`${this.#apiUrlRoot}roles`);
  }

  getUserRoles(userId: string) {
    return this.#http.get<Array<string>>(`${this.#apiUrlRoot}users/${userId}/roles`);
  }

  getUsersInRole(role: string) {
    return this.#http.get<Array<AdminUser>>(`${this.#apiUrlRoot}roles/${role}`);
  }

  addUserToRole(userId: string, role: string) {
    return this.#http.post<boolean>(`${this.#apiUrlRoot}roles/${role}/${userId}`, null);
  }

  removeUserFromRole(userId: string, role: string) {
    return this.#http.delete<boolean>(`${this.#apiUrlRoot}roles/${role}/${userId}`);
  }

  lockUserAccount(userId: string) {
    return this.#http.post<boolean>(`${this.#apiUrlRoot}users/${userId}/lock`, null);
  }

  unlockUserAccount(userId: string) {
    return this.#http.post<boolean>(`${this.#apiUrlRoot}users/${userId}/unlock`, null);
  }
}
