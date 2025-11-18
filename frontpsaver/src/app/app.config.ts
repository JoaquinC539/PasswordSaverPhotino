import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withDisabledInitialNavigation, withHashLocation } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { LocationStrategy } from '@angular/common';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes), provideHttpClient(withFetch()) ]
};
