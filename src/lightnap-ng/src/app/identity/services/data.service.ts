import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT } from "@core";
import { LoginRequest, LoginSuccessResult, NewPasswordRequest, RegisterRequest, ResetPasswordRequest, SendVerificationEmailRequest, VerifyCodeRequest, VerifyEmailRequest } from "@identity";

@Injectable({
  providedIn: "root",
})
export class DataService {
  #http = inject(HttpClient);
  #identityApiUrlRoot = `${inject(API_URL_ROOT)}identity/`;

  getAccessToken() {
    return this.#http.get<string>(`${this.#identityApiUrlRoot}access-token`);
  }

  logIn(loginRequest: LoginRequest) {
    return this.#http.post<LoginSuccessResult>(`${this.#identityApiUrlRoot}login`, loginRequest);
  }

  register(registerRequest: RegisterRequest) {
    return this.#http.post<LoginSuccessResult>(`${this.#identityApiUrlRoot}register`, registerRequest);
  }

  logOut() {
    return this.#http.get<boolean>(`${this.#identityApiUrlRoot}logout`);
  }

  resetPassword(resetPasswordRequest: ResetPasswordRequest) {
    return this.#http.post<boolean>(`${this.#identityApiUrlRoot}reset-password`, resetPasswordRequest);
  }

  newPassword(newPasswordRequest: NewPasswordRequest) {
    return this.#http.post<LoginSuccessResult>(`${this.#identityApiUrlRoot}new-password`, newPasswordRequest);
  }

  verifyCode(verifyCodeRequest: VerifyCodeRequest) {
    return this.#http.post<string>(`${this.#identityApiUrlRoot}verify-code`, verifyCodeRequest);
  }

  requestVerificationEmail(sendVerificationEmailRequest: SendVerificationEmailRequest) {
    return this.#http.post<boolean>(`${this.#identityApiUrlRoot}request-verification-email`, sendVerificationEmailRequest);
  }

  verifyEmail(verifyEmailRequest: VerifyEmailRequest) {
    return this.#http.post<boolean>(`${this.#identityApiUrlRoot}verify-email`, verifyEmailRequest);
  }
}
