import { Component, inject, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { AuthService } from './services/auth.service';
import { filter } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ProfileService } from './services/profile.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  private authService = inject(AuthService);
  private router = inject(Router);
  private profileService = inject(ProfileService);

  isLoaded = true;
  user = {};

  ngOnInit(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.handleAuthenticationRedirect();
    });

    this.loadProfile();
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  private handleAuthenticationRedirect(): void {
    const publicRoutes = ['/login', '/register'];
    const currentUrl = this.router.url;
    
    if (!this.isAuthenticated() && !publicRoutes.includes(currentUrl)) {
      this.router.navigate(['/login']);
    } else if (this.isAuthenticated() && publicRoutes.includes(currentUrl)) {
      this.router.navigate(['/lists']); 
    }
  }

  private loadProfile() {
    this.isLoaded = false;

    this.profileService.getProfile().subscribe({
      next: (data) => {
        this.user = data;
        this.isLoaded = true;
      },
      error: (error) =>  console.error('Error loading profile:', error)
    });
  }
}
