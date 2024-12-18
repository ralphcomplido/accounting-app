import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT, PagedResponse } from "@core";
import {
  ApplicationSettings,
  ChangeEmailRequest,
  ChangePasswordRequest,
  ConfirmChangeEmailRequest,
  Device,
  Profile,
  Notification,
  SearchNotificationsRequest,
  UpdateProfileRequest,
} from "@profile";
import { DeviceHelper } from "@profile/helpers/device.helper";
import { NotificationHelper } from "@profile/helpers/notification.helper";
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

  searchNotifications(searchNotificationsRequest: SearchNotificationsRequest) {
    return this.#http
      .post<PagedResponse<Notification>>(`${this.#apiUrlRoot}notifications`, searchNotificationsRequest)
      .pipe(tap(results => results.data.forEach(NotificationHelper.rehydrate)));
  }

  markAllNotificationsAsRead() {
    return this.#http.put<boolean>(`${this.#apiUrlRoot}notifications/mark-all-as-read`, undefined);
  }

  markNotificationAsRead(id: number) {
    return this.#http.put<boolean>(`${this.#apiUrlRoot}notifications/${id}/mark-as-read`, undefined);
  }
}
