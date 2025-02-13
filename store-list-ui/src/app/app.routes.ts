import { Routes } from '@angular/router';
import { ShoppingListComponent } from './components/shopping-list/shopping-list.component';
import { ShoppingListFormComponent } from './components/shopping-list-form/shopping-list-form.component';
import { ShoppingListDetailsComponent } from './components/shopping-list-details/shopping-list-details.component';

export const routes: Routes = [
    { path: '', redirectTo: 'lists', pathMatch: 'full' },
    { path: 'lists', component: ShoppingListComponent },
    { path: 'lists/:id', component: ShoppingListDetailsComponent },
    { path: 'new', component: ShoppingListFormComponent },
    { path: 'edit/:id', component: ShoppingListFormComponent }
];
