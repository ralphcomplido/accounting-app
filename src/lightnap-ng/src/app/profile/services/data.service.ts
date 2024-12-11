import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT } from "@core";
import { ApplicationSettings, ChangeEmailRequest, ChangePasswordRequest, ConfirmChangeEmailRequest, Device, Profile, UpdateProfileRequest } from "@profile";
import { DeviceHelper } from "@profile/helpers/device.helper";
import { tap } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class DataService {
  #http = inject(HttpClient);
  #apiUrlRoot = `${inject(API_URL_ROOT)}profile/`;

  changePassword(changePasswordRequest: ChangePasswordRequest) {
    return this.#http.post<boolean>(`${this.#apiUrlRoot}change-password`, changePasswordRequest);
  }

  changeEmail(changeEmailRequest: ChangeEmailRequest) {
    return this.#http.post<boolean>(`${this.#apiUrlRoot}change-email`, changeEmailRequest);
  }

  confirmEmailChange(confirmChangeEmailRequest: ConfirmChangeEmailRequest) {
    return this.#http.post<boolean>(`${this.#apiUrlRoot}confirm-email-change`, confirmChangeEmailRequest);
  }

  getProfile() {
    return this.#http.get<Profile>(`${this.#apiUrlRoot}`);
  }

  updateProfile(updateProfile: UpdateProfileRequest) {
    return this.#http.put<Profile>(`${this.#apiUrlRoot}`, updateProfile);
  }

  getDevices() {
    return this.#http
      .get<Array<Device>>(`${this.#apiUrlRoot}devices`)
      .pipe(tap(devices => devices.forEach(device => DeviceHelper.rehydrate(device))));
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
