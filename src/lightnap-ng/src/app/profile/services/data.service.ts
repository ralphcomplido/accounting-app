import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT, ApiResponse } from "@core";
import { ApplicationSettings, ChangePasswordRequest, Device, Profile, UpdateProfileRequest } from "@profile";

@Injectable({
  providedIn: "root",
})
export class DataService {
  #http = inject(HttpClient);
  #apiUrlRoot = `${inject(API_URL_ROOT)}profile/`;

  changePassword(changePasswordRequest: ChangePasswordRequest) {
    return this.#http.post<boolean>(`${this.#apiUrlRoot}change-password`, changePasswordRequest);
  }

  getProfile() {
    return this.#http.get<Profile>(`${this.#apiUrlRoot}`);
  }

  updateProfile(updateProfile: UpdateProfileRequest) {
    return this.#http.put<Profile>(`${this.#apiUrlRoot}`, updateProfile);
  }

  getDevices() {
    return this.#http.get<Device>(`${this.#apiUrlRoot}devices`);
  }

  revokeDevice(deviceId: string) {
    return this.#http.delete<boolean>(`${this.#apiUrlRoot}devices/${deviceId}`);
  }

  getSettings() {
    return this.#http.get<ApplicationSettings>(`${this.#apiUrlRoot}settings`);
  }

  updateSettings(browserSettings: ApplicationSettings) {
    return this.#http.put<boolean>(`${this.#apiUrlRoot}settings`, browserSettings);
  }
}
