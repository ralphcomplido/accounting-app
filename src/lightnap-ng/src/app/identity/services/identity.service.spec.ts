import { TestBed } from '@angular/core/testing';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ErrorApiResponse, SuccessApiResponse, TimerService } from '@core';
import { of } from 'rxjs';
import { DataService } from './data.service';
import { IdentityService } from './identity.service';
import { RegisterRequest, VerifyCodeRequest, ResetPasswordRequest, NewPasswordRequest } from '@identity';
import { InitializationService } from '@core/services/initialization.service';

describe('IdentityService', () => {
    let service: IdentityService;
    let initializationServiceSpy: jasmine.SpyObj<InitializationService>;
    let dataServiceSpy: jasmine.SpyObj<DataService>;
    let timerServiceSpy: jasmine.SpyObj<TimerService>;
    // Using a valid JWT token for testing purposes. IdentityService will try to parse it so it might as well work.
    const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

    beforeEach(() => {
        const dataSpy = jasmine.createSpyObj('DataService', ['getAccessToken', 'logIn', 'register', 'logOut', 'verifyCode', 'resetPassword', 'newPassword']);
        const timerSpy = jasmine.createSpyObj('TimerService', ['watchTimer$']);
        const initializationSpy = jasmine.createSpyObj('InitializationService', ['initialized$']);

        TestBed.configureTestingModule({
            providers: [
                IdentityService,
                { provide: InitializationService, useValue: initializationSpy },
                { provide: DataService, useValue: dataSpy },
                { provide: TimerService, useValue: timerSpy },
                JwtHelperService
            ]
        });

        initializationServiceSpy = TestBed.inject(InitializationService) as jasmine.SpyObj<InitializationService>;
        Object.defineProperty(initializationServiceSpy, 'initialized$', { get: () => of(true) });

        timerServiceSpy = TestBed.inject(TimerService) as jasmine.SpyObj<TimerService>;
        timerServiceSpy.watchTimer$.and.returnValue(of(0));

        dataServiceSpy = TestBed.inject(DataService) as jasmine.SpyObj<DataService>;
        dataServiceSpy.getAccessToken.and.returnValue(of(undefined));

        service = TestBed.inject(IdentityService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should initialize and try to refresh token', () => {
        service = TestBed.inject(IdentityService);
        expect(timerServiceSpy.watchTimer$).toHaveBeenCalled();
        expect(dataServiceSpy.getAccessToken).toHaveBeenCalled();
    });

    it('should log in and set token', () => {
        const loginRequest = {} as any;
        dataServiceSpy.logIn.and.returnValue(of({ bearerToken: token, twoFactorRequired: false }));
        service.logIn(loginRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.logIn).toHaveBeenCalledWith(loginRequest);
    });

    it('should log out and clear token', () => {
        dataServiceSpy.logOut.and.returnValue(of(true));
        service.logOut().subscribe(() => {
            expect(service.getBearerToken()).toBeUndefined();
        });
        expect(dataServiceSpy.logOut).toHaveBeenCalled();
    });

    it('should register and set token', () => {
        const registerRequest: RegisterRequest = {} as any;
        dataServiceSpy.register.and.returnValue(of({ bearerToken: token, twoFactorRequired: false }));
        service.register(registerRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.register).toHaveBeenCalledWith(registerRequest);
    });

    it('should verify code and set token', () => {
        const verifyCodeRequest: VerifyCodeRequest = {} as any;
        dataServiceSpy.verifyCode.and.returnValue(of(token));
        service.verifyCode(verifyCodeRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.verifyCode).toHaveBeenCalledWith(verifyCodeRequest);
    });

    it('should reset password', () => {
        const resetPasswordRequest: ResetPasswordRequest = <any>{ };
        dataServiceSpy.resetPassword.and.returnValue(of({} as any));
        service.resetPassword(resetPasswordRequest).subscribe();
        expect(dataServiceSpy.resetPassword).toHaveBeenCalledWith(resetPasswordRequest);
    });

    it('should set new password and set token', () => {
        const newPasswordRequest: NewPasswordRequest = {} as any;
        dataServiceSpy.newPassword.and.returnValue(of(token));
        service.newPassword(newPasswordRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.newPassword).toHaveBeenCalledWith(newPasswordRequest);
    });
});
