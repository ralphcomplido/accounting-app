import { LocationStrategy, PathLocationStrategy } from "@angular/common";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { APP_INITIALIZER, enableProdMode, importProvidersFrom } from "@angular/core";
import { bootstrapApplication, BrowserModule } from "@angular/platform-browser";
import { provideAnimations } from "@angular/platform-browser/animations";
import { provideRouter, withRouterConfig } from "@angular/router";
import { API_URL_ROOT } from "@core";
import { apiResponseInterceptor } from "@core/interceptors/api-response-interceptor";
import { tokenInterceptor } from "@core/interceptors/token-interceptor";
import { InitializationService } from "@core/services/initialization.service";
import { RouteConfig, Routes } from "@routing/routes";
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
    provideRouter(Routes, withRouterConfig(RouteConfig)),
    importProvidersFrom(BrowserModule),
    InitializationService,
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [InitializationService],
      multi: true,
    },
    provideAnimations(),
    { provide: API_URL_ROOT, useValue: environment.apiUrlRoot },
    { provide: LocationStrategy, useClass: PathLocationStrategy },
    provideHttpClient(withInterceptors([tokenInterceptor, apiResponseInterceptor])),
    MessageService,
    ConfirmationService,
  ],
}).catch(err => console.error("Error bootstrapping application:", err));
