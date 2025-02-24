import { Routes } from '@angular/router';
import { ShoppingListComponent } from './components/shopping-list/shopping-list.component';
import { ShoppingListFormComponent } from './components/shopping-list-form/shopping-list-form.component';
import { ShoppingListDetailsComponent } from './components/shopping-list-details/shopping-list-details.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth.guard';
import { ProfileComponent } from './components/profile/profile.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'lists', pathMatch: 'full' },
      { path: 'lists', component: ShoppingListComponent },
      { path: 'lists/:id', component: ShoppingListDetailsComponent },
      { path: 'new', component: ShoppingListFormComponent },
      { path: 'edit/:id', component: ShoppingListFormComponent },
      { path: 'profile', component: ProfileComponent }
    ]
  }   
];
