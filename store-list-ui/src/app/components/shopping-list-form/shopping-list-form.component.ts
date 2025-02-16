import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ShoppingListService } from '../../services/shopping-list.service';
import { CommonModule } from '@angular/common';

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

  shoppingListForm!: FormGroup;
  isEditMode = false;
  listId: string | null = null;
  submitted = false; 
  successMessage: string | null = null; 
  errorMessage: string | null = null; 
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

  createItemFormGroup() {
    return this.fb.group({
      name: ['', Validators.required],
      quantity: [1, Validators.required]
    });
  }

  private loadListData(): void {
    this.shoppingListService.getListById(this.listId!).subscribe((list) => {
      this.shoppingListForm.patchValue({
        name: list.name,
      });

      list.items.forEach((item) => {
        this.items.push(this.fb.group({
          name: [item.name, Validators.required],
          quantity: [item.quantity, Validators.required]
        }));
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

    const formData = this.shoppingListForm.value;

    if (this.isEditMode) {
      const updateData = {
        id: this.listId,
        ...formData
      };

      this.shoppingListService.updateList(this.listId!, updateData).subscribe({
        next: () => {
          this.successMessage = 'List updated successfully!';
          setTimeout(() => this.router.navigate(['/lists']), 2000);
        },
        error: (error) => {
          this.errorMessage = 'Error updating list. Please try again.';
          console.error('Error:', error);
        }
      });
    } else {
      this.shoppingListService.addList(formData).subscribe({
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
  }

  confirmDelete(): void {
    if(!this.listId) return;

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
