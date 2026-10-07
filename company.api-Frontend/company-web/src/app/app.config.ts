import { ApplicationConfig, inject, provideAppInitializer, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { LanguageService } from './core/services/language.service';
export const appConfig: ApplicationConfig = {
  providers: [
    provideAppInitializer(() => inject(LanguageService).initialize()),
    provideZonelessChangeDetection(), // default for new Angular 22 projects — no Zone.js
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])) // interceptors added in Part B
  ]
};