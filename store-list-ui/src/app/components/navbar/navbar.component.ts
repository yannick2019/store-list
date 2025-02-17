import { CommonModule } from '@angular/common';
import { Component, inject, Input } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

interface User {
  name: string;
  email: string;
}

@Component({
  selector: 'app-navbar',
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  @Input() user: User | null = null;
  private router = inject(Router);
  isDropdownOpen = false;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  login(): void {
    // ToDo
  }

  logout(): void {
    // ToDo
    // Implement logic
    this.isDropdownOpen = false;
  }

  goToProfile(): void {
    this.router.navigate(['/profile']);
  }
}
