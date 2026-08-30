import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginRequest, LoginResponse } from '../models/auth.model';
import { environment } from '../../environments/environment';
import { NotificationService } from './notification.service';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly tokenKey = 'auth_token';
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/login`;
  private router = inject(Router);
  private notificationService = inject(NotificationService);

  login(req: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.baseUrl, req);
  }

  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
    this.notificationService.connect();
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.notificationService.disconnect();
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
