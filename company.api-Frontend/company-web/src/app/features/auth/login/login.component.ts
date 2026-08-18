import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormField, FormRoot, form } from '@angular/forms/signals';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { LoginRequest } from '../../../core/models/auth.model';
import { firstValueFrom } from 'rxjs/internal/firstValueFrom';
import { HttpErrorResponse } from '@angular/common/http';
import { loginSchema } from './login-schema';

const EMPTY_USER: LoginRequest = {
  username: '', password: ''
};
@Component({
  selector: 'app-login',
  imports: [CommonModule, FormField, FormRoot],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  protected errorMessage = signal<string | null>(null);
  protected readonly model = signal<LoginRequest>(EMPTY_USER);
  protected readonly loginForm = form(this.model, loginSchema, {
    submission: {
      action: async (f) => this.login(f().value()),
      onInvalid: () => {
        this.errorMessage.set('Form is invalid — fix the highlighted fields.');
        console.warn('Form is invalid — fix the highlighted fields.');
      }
    }
  });
  private async login(value: LoginRequest): Promise<void> {
    this.errorMessage.set(null);
    try {
      const response = await firstValueFrom(this.authService.login(value));
      if (response && response.token) {
        this.authService.saveToken(response.token);
        const returnUrl = '/employees';
        this.router.navigate([returnUrl]);
      } else {
        this.errorMessage.set('Login failed: No token received.');
      }
    } catch (error) {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        this.errorMessage.set('Invalid username or password.');
      } else {
        this.errorMessage.set('An unexpected error occurred during login.');
      }
    }
  }
}
