import { Component, inject, OnInit } from '@angular/core';
import { ShoppingListService } from '../../services/shopping-list.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-shopping-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './shopping-list.component.html',
  styleUrl: './shopping-list.component.css'
})
export class ShoppingListComponent implements OnInit {
  shoppingLists: any[] = [];
  isLoading = false;
  error: string | null = null;

  private shoppingListService = inject(ShoppingListService);
  private authService = inject(AuthService);

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      return;
    }

    this.loadLists();
  }

  private loadLists() {
    this.isLoading = true;
    this.error = null;

    this.shoppingListService.getAllLists().subscribe({
      next: (data) => {
        this.shoppingLists = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Failed to load shopping lists';
        this.isLoading = false;
        console.error('Error loading lists:', error);
      }
    })
  }
}
