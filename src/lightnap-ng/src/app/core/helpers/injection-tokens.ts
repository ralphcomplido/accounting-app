import { InjectionToken } from "@angular/core";

/**
 * Injection token for the root URL of the API.
 *
 * This token can be used to inject the root URL of the API into Angular services or components.
 *
 * @example
 * ```typescript
 * constructor(@Inject(API_URL_ROOT) private apiUrlRoot: string) {}
 * ```
 */
export const API_URL_ROOT = new InjectionToken<string>('API_URL_ROOT');

/**
 * Injection token for the name of the application.
 *
 * This token can be used to inject the name of the application into Angular services or components, such as the document title strategy.
 *
 * @example
 * ```typescript
 * constructor(@Inject(APP_NAME) private appName: string) {}
 * ```
 */
export const APP_NAME = new InjectionToken<string>('APP_NAME');
