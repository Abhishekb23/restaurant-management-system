import {
  Component,
  inject
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import {
  MatCardModule
} from '@angular/material/card';

import {
  MatInputModule
} from '@angular/material/input';

import {
  MatButtonModule
} from '@angular/material/button';

import {
  MatIconModule
} from '@angular/material/icon';

import {
  MatDividerModule
} from '@angular/material/divider';

import { AuthService }
from '../services/auth.service';

@Component({
  selector: 'app-login',

  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule
  ],

  templateUrl: './login.html'
})
export class Login {

  private fb =
    inject(FormBuilder);

  private auth =
    inject(AuthService);

  private router =
    inject(Router);

  private route =
    inject(ActivatedRoute);

  loading = false;

  isRegister = false;

  isOtp = false;

  constructor()
  {
    this.route.queryParams
      .subscribe(params =>
      {
        const token =
          params['token'];

        if (token)
        {
          localStorage.setItem(
            'token',
            token
          );

          this.router.navigate([
            '/dashboard'
          ]);
        }
      });
  }

  // LOGIN FORM

  form = this.fb.group({
    email: [
      '',
      [
        Validators.required,
        Validators.email
      ]
    ],

    password: [
      '',
      Validators.required
    ]
  });

  // REGISTER FORM

  registerForm = this.fb.group({
    fullName: [
      '',
      Validators.required
    ],

    email: [
      '',
      [
        Validators.required,
        Validators.email
      ]
    ],

    password: [
      '',
      Validators.required
    ]
  });

  // OTP FORM

  otpForm = this.fb.group({
    destination: [
      '',
      [
        Validators.required,
        Validators.email
      ]
    ],

    code: [
      '',
      Validators.required
    ]
  });

  // LOGIN

  login()
  {
    if (this.form.invalid)
      return;

    this.loading = true;

    this.auth.login(
      this.form.value
    )
    .subscribe({
      next: () =>
      {
        this.router.navigate([
          '/dashboard'
        ]);
      },

      error: () =>
      {
        this.loading = false;

        alert(
          'Invalid credentials'
        );
      }
    });
  }

  // REGISTER

  register()
  {
    if (this.registerForm.invalid)
      return;

    this.auth.register(
      this.registerForm.value
    )
    .subscribe({
      next: () =>
      {
        alert(
          'Registration successful'
        );

        this.isRegister = false;
      },

      error: () =>
      {
        alert(
          'Registration failed'
        );
      }
    });
  }

  // SEND OTP

  sendOtp()
  {
    const email =
      this.otpForm.value.destination;

    if (!email)
      return;

    this.auth.sendOtp(email)
      .subscribe({
        next: () =>
        {
          alert(
            'OTP sent successfully'
          );
        },

        error: () =>
        {
          alert(
            'Failed to send OTP'
          );
        }
      });
  }

  // VERIFY OTP

  verifyOtp()
  {
    if (this.otpForm.invalid)
      return;

    this.auth.verifyOtp(
      this.otpForm.value
    )
    .subscribe({
      next: (res: any) =>
      {
        localStorage.setItem(
          'token',
          res.token
        );

        this.router.navigate([
          '/dashboard'
        ]);
      },

      error: () =>
      {
        alert(
          'Invalid OTP'
        );
      }
    });
  }

  // GOOGLE LOGIN

  googleLogin()
  {
    this.auth.googleLogin();
  }

  // TOGGLES

  toggleRegister()
  {
    this.isRegister =
      !this.isRegister;

    this.isOtp = false;
  }

  toggleOtp()
  {
    this.isOtp =
      !this.isOtp;

    this.isRegister = false;
  }
}