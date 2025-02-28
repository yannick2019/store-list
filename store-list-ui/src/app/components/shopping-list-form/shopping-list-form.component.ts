import { Component, OnInit, inject } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { take } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { ShoppingListService } from '../../services/shopping-list.service';
import { UserStateService } from '../../services/user-state.service';
import { ShoppingList, ShoppingListItem } from '../../models/models';

@Component({
  selector: 'app-shopping-list-form',
  standalone: true,
  templateUrl: './shopping-list-form.component.html',
  styleUrls: ['./shopping-list-form.component.css'],
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
})
export class ShoppingListFormComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private shoppingListService = inject(ShoppingListService);
  private fb = inject(FormBuilder);
  private userStateService = inject(UserStateService);

  shoppingListForm!: FormGroup;
  isEditMode = false;
  listId: string | null = null;
  submitted = false;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  userId: string | undefined;
  showDeleteModal = false;

  ngOnInit(): void {
    this.listId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.listId;

    this.shoppingListForm = this.fb.group({
      name: ['', Validators.required],
      items: this.fb.array([])
    });

    if (this.isEditMode) {
      this.loadListData();
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.shoppingListForm.get(fieldName);
    return field ? (field.invalid && (field.dirty || field.touched || this.submitted)) : false;
  }

  isItemInvalid(index: number): boolean {
    const itemsArray = this.shoppingListForm.get('items') as FormArray;
    const item = itemsArray.at(index);
    return item ? (item.invalid && (item.dirty || item.touched || this.submitted)) : false;
  }

  get items(): FormArray {
    return this.shoppingListForm.get('items') as FormArray;
  }

  createItemFormGroup(item?: ShoppingListItem | null) {
    return this.fb.group({
      id: [item?.id || ''],
      name: [item?.name || '', Validators.required],
      quantity: [item?.quantity || 1, [Validators.required, Validators.min(1)]],
      isChecked: [item?.isChecked || false],
      shoppingListId: [item?.shoppingListId || '']
    });
  }

  private loadListData(): void {
    this.shoppingListService.getListById(this.listId!).subscribe((list) => {
      this.shoppingListForm.patchValue({
        name: list.name,
      });

      // Clear the items array first in case there's any leftover data
      while (this.items.length) {
        this.items.removeAt(0);
      }

      // Add each item with its ID
      list.items.forEach((item) => {
        this.items.push(this.createItemFormGroup(item));
      });
    });
  }

  addItem(): void {
    this.items.push(this.createItemFormGroup());
  }

  removeItem(index: number): void {
    this.items.removeAt(index);
  }

  deleteList(): void {
    this.showDeleteModal = true;
  }

  submit(): void {
    this.submitted = true;
    this.successMessage = null;
    this.errorMessage = null;

    if (this.shoppingListForm.invalid) {
      this.errorMessage = 'Please check the form for errors.';
      return;
    }

    if (this.items.length === 0) {
      this.errorMessage = 'Please add at least one item to the list.';
      return;
    }

    this.userStateService.user$.pipe(take(1)).subscribe(user => {
      this.userId = user?.id;

      if (!this.userId) {
        this.errorMessage = 'User not authenticated.';
        return;
      }

      const formData = this.shoppingListForm.value;

      if (this.isEditMode) {
        const updateData: ShoppingList = {
          id: this.listId!,
          name: formData.name,
          items: formData.items.map((item: any) => {
            // Only include items with an ID (existing items) or with filled in data (new items)
            if (item.id || (item.name && item.name.trim() !== '')) {
              return {
                // For new items, don't send an ID at all instead of sending an empty string
                ...(item.id ? { id: item.id } : {}),
                name: item.name,
                quantity: Number(item.quantity), 
                isChecked: Boolean(item.isChecked), 
                shoppingListId: this.listId 
              };
            }
            return null;
          }).filter((item: ShoppingListItem | null): item is ShoppingListItem => item !== null), // Remove any null items
          userId: this.userId,
        };

        //console.log('updateData:', updateData);

        this.shoppingListService.updateList(this.listId!, updateData).subscribe({
          next: (response) => {
            //console.log('Response after update:', response);
            this.successMessage = 'List updated successfully!';
            setTimeout(() => this.router.navigate(['/lists']), 2000);
          },
          error: (error) => {
            console.error('Error details:', error);
            if (error.error && error.error.errors) {
              // Display detailed validation errors if available
              const errorKeys = Object.keys(error.error.errors);
              if (errorKeys.length > 0) {
                this.errorMessage = `Validation error: ${error.error.errors[errorKeys[0]]}`;
              } else {
                this.errorMessage = 'Error updating list. Please try again.';
              }
            } else {
              this.errorMessage = 'Error updating list. Please try again.';
            }
          }
        });
      } else {
        const newList = {
          name: formData.name,
          items: formData.items.map((item: any) => ({
            name: item.name,
            quantity: Number(item.quantity),
            isChecked: Boolean(item.isChecked)
          })),
          userId: this.userId
        };

        this.shoppingListService.addList(newList).subscribe({
          next: () => {
            this.successMessage = 'List created successfully!';
            setTimeout(() => this.router.navigate(['/lists']), 2000);
          },
          error: (error) => {
            this.errorMessage = 'Error creating list. Please try again.';
            console.error('Error:', error);
          }
        });
      }
    });
  }

  confirmDelete(): void {
    if (!this.listId) return;

    this.shoppingListService.deleteList(this.listId).subscribe({
      next: () => {
        this.successMessage = 'List deleted successfully!';
        this.showDeleteModal = false;
        setTimeout(() => this.router.navigate(['/lists']), 2000);
      },
      error: (error) => {
        this.errorMessage = error.error || 'Error deleting list. Please try again.';
        this.showDeleteModal = false;
        console.error('Error:', error);
      }
    });
  }
}