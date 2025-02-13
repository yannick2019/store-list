import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ShoppingListService } from '../../services/shopping-list.service';

interface ShoppingListItem {
  id: string;
  name: string;
  quantity: number;
  checked?: boolean;
}

interface ShoppingList {
  id: string;
  name: string;
  items: ShoppingListItem[];
}

@Component({
  selector: 'app-shopping-list-details',
  imports: [CommonModule, RouterModule],
  templateUrl: './shopping-list-details.component.html',
  styleUrl: './shopping-list-details.component.css'
})
export class ShoppingListDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private shoppingListService = inject(ShoppingListService);

  shoppingList?: ShoppingList;
  isLoading = true;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.shoppingListService.getListById(id).subscribe({
        next: (list) => {
          const checkedStates = JSON.parse(localStorage.getItem(`checkedItems_${list.id}`) || '{}');
          
          this.shoppingList = {
            ...list,
            items: list.items.map(item => ({
              ...item,
              checked: checkedStates[item.id] || false
            }))
          };
          this.isLoading = false;
        },
        error: () => {
          this.router.navigate(['/lists']);
        }
      });
    }
  }

  toggleItemCheck(item: ShoppingListItem): void {
    item.checked = !item.checked;
    
    // Sauvegarder dans localStorage
    if (this.shoppingList) {
      const checkedStates = JSON.parse(localStorage.getItem(`checkedItems_${this.shoppingList.id}`) || '{}');
      checkedStates[item.id] = item.checked;
      localStorage.setItem(`checkedItems_${this.shoppingList.id}`, JSON.stringify(checkedStates));
    }
  }

  getCheckedItemsCount(): number {
    return this.shoppingList?.items.filter(item => item.checked).length || 0;
  }
}
