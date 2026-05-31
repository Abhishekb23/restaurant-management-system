import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-user-auth',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './user-auth.html'
})
export class UserAuth {

  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  hidePassword = true;

  mode: 'login' | 'register' | 'otp' = 'login';

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  registerForm = this.fb.group({
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  otpForm = this.fb.group({
    destination: ['', [Validators.required, Validators.email]],
    code: [
      '',
      [
        Validators.required,
        Validators.pattern(/^[0-9]{6}$/) // exactly 6 digits
      ]
    ]
  });

  login() {
    this.auth.login(this.loginForm.value).subscribe(() => {
      this.router.navigate(['/dashboard']);
    });
  }

  register() {
    this.auth.register(this.registerForm.value).subscribe(() => {
      this.mode = 'login';
    });
  }

  sendOtp() {
    this.auth.sendOtp(this.otpForm.value.destination!).subscribe();
  }

  verifyOtp() {
    this.auth.verifyOtp(this.otpForm.value).subscribe((res: any) => {
      localStorage.setItem('token', res.token);
      this.router.navigate(['/dashboard']);
    });
  }

  googleLogin() {
    this.auth.googleLogin();
  }

  redirectToGoogleLogin(): void {
    console.log("btn clicked");
    window.location.href = 'https://localhost:7280/api/auth/google-login';
  }
}