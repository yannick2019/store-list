import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserStateService } from '../../services/user-state.service';
import { User } from '../../models/models';
import { ProfileService } from '../../services/profile.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit{
  user: User | null = null;
  editableUser: { firstName: string, lastName: string } = { firstName: '', lastName: '' };
  isEditMode = false;
  isSaving = false;

  private userStateService = inject(UserStateService);
  private profileService = inject(ProfileService);

  ngOnInit(): void {
    this.userStateService.user$.subscribe(user => {
      this.user = user;
      if(user) {
        this.editableUser = {
          firstName: user.firstName || '',
          lastName: user.lastName || ''
        }
      }
    });
  }

  enableEditMode(): void {
    this.isEditMode = true;
  }

  cancelEditMode(): void {
    if (this.user) {
      this.editableUser = {
        firstName: this.user.firstName || '',
        lastName: this.user.lastName || ''
      };
    }
    this.isEditMode = false;
  }

  saveProfile(): void {
    if (!this.user) return;
    
    this.isSaving = true;
    
    this.profileService.updateProfile(this.editableUser).subscribe({
      next: (updatedUser) => {
        this.userStateService.setUser(updatedUser);
        this.isEditMode = false;
        this.isSaving = false;
      },
      error: (error) => {
        console.error('Error updating profile:', error);
        this.isSaving = false;
      }
    });
  }
}
