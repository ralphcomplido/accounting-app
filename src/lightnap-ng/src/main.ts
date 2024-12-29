import { LocationStrategy, PathLocationStrategy } from "@angular/common";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { APP_INITIALIZER, enableProdMode, importProvidersFrom } from "@angular/core";
import { bootstrapApplication, BrowserModule } from "@angular/platform-browser";
import { provideAnimations } from "@angular/platform-browser/animations";
import { provideRouter, TitleStrategy, withComponentInputBinding, withInMemoryScrolling, withRouterConfig } from "@angular/router";
import { API_URL_ROOT, APP_NAME } from "@core";
import { apiResponseInterceptor } from "@core/interceptors/api-response-interceptor";
import { tokenInterceptor } from "@core/interceptors/token-interceptor";
import { InitializationService } from "@core/services/initialization.service";
import { PrependNameTitleStrategy } from "@core/strategies/prepend-name-title.strategy";
import { Routes } from "@routing/routes";
import { ConfirmationService, MessageService } from "primeng/api";
import { AppComponent } from "./app/app.component";
import { environment } from "./environments/environment";

if (environment.production) {
  enableProdMode();
}

export function initializeApp(initializationService: InitializationService) {
  return () => initializationService.initialize();
}

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(Routes, withInMemoryScrolling(), withComponentInputBinding(), withRouterConfig({})),
    importProvidersFrom(BrowserModule),
    InitializationService,
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [InitializationService],
      multi: true,
    },
    provideAnimations(),
    { provide: API_URL_ROOT, useValue: environment.apiUrlRoot ?? (() => { throw new Error("Required setting 'environment.apiUrlRoot' root is not defined."); })() },
    { provide: APP_NAME, useValue: environment.appName },
    { provide: LocationStrategy, useClass: PathLocationStrategy },
    { provide: TitleStrategy, useClass: PrependNameTitleStrategy },
    provideHttpClient(withInterceptors([tokenInterceptor, apiResponseInterceptor])),
    MessageService,
    ConfirmationService,
  ],
}).catch(err => console.error("Error bootstrapping application:", err));
