import { ApplicationConfig, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(), // default for new Angular 22 projects — no Zone.js
    provideRouter(routes),
    provideHttpClient(withInterceptors([])) // interceptors added in Part B
  ]
};