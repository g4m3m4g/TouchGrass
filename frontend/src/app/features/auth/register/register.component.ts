import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  registerForm = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  onSubmit(): void {
    if (this.registerForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.register(
      this.registerForm.getRawValue() as { username: string; email: string; password: string }
    ).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = 'Account created. Taking you to sign in…';
        setTimeout(() => this.router.navigate(['/login']), 1200);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Something went wrong. Please try again.';
      },
    });
  }
}
