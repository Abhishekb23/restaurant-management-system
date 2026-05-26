import { Injectable, inject }
from '@angular/core';

import {
  HttpClient
} from '@angular/common/http';

import {
  Observable,
  tap
} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http =
    inject(HttpClient);

  private api =
    'https://backend-cbdg.onrender.com/api/auth';

  login(data: any): Observable<any> {
    return this.http.post(
      `${this.api}/login`,
      data
    ).pipe(
      tap((res: any) =>
      {
        localStorage.setItem(
          'token',
          res.token
        );
      })
    );
  }

  register(data: any): Observable<any> {
    return this.http.post(
      `${this.api}/register`,
      data
    );
  }

  googleLogin()
  {
    window.location.href =
      `${this.api}/google-login`;
  }

  logout()
  {
    localStorage.removeItem(
      'token'
    );
  }

  getToken()
  {
    return localStorage.getItem(
      'token'
    );
  }

  isLoggedIn()
  {
    return !!this.getToken();
  }
  sendOtp(email: string)
  {
    return this.http.post(
      `${this.api}/send-otp`,
      {
        destination: email,
  
        type: 'EMAIL'
      }
    );
  }

verifyOtp(data: any)
{
  return this.http.post(
    `${this.api}/verify-otp`,
    {
      destination:
        data.destination,

      code:
        data.code,

      type:
        'EMAIL'
    }
  );
}}