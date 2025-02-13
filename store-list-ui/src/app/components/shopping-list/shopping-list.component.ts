import { Component, inject, OnInit } from '@angular/core';
import { ShoppingListService } from '../../services/shopping-list.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-shopping-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './shopping-list.component.html',
  styleUrl: './shopping-list.component.css'
})
export class ShoppingListComponent implements OnInit {
  shoppingLists: any[] = [];
  private shoppingListService = inject(ShoppingListService);

  ngOnInit(): void {
    this.shoppingListService.getAllLists().subscribe(data => {
      this.shoppingLists = data;
    });
  }
}
