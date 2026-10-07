import { Component, OnInit, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { NotificationService } from './core/services/notification.service';
import { AuthService } from './core/services/auth.service';
import { LanguageService } from './core/services/language.service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  private notificationService = inject(NotificationService);
  private authService = inject(AuthService);
  private languageService = inject(LanguageService);
  readonly isArabic = this.languageService.isArabic;

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.notificationService.connect();
    }
  }

  toggleLanguage(): void {
    this.languageService.toggleLanguage();
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  logout(): void {
    this.authService.logout();
  }
}