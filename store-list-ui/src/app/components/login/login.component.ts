import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  loginForm!: FormGroup;
  submitted = false;
  successMessage: string | null = null; 
  errorMessage: string | null = null; 

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.loginForm.get(fieldName);
    return !!(field && field.invalid && (field.touched || this.submitted));
  }

  onSubmit(): void {
    this.submitted = true;
    this.successMessage = null;
    this.errorMessage = null;

    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: () => {
          this.successMessage = 'Login successful! Redirecting...';
          setTimeout(() => this.router.navigate(['/lists']), 2000);
        },
        error: () => this.errorMessage = 'Invalid username or password'
      })
    }
  }

  signInWithGoogle(): void {
    // Implement Google login
  }
}
