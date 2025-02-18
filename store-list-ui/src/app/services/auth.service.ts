import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Authresponse, LoginRequest, RegisterRequest } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7019/api/account';
  private http = inject(HttpClient);
  private tokenSubjet = new BehaviorSubject<string | null>(localStorage.getItem('token'));

  login(request: LoginRequest): Observable<Authresponse> {
    return this.http.post<Authresponse>(`${this.apiUrl}/login`, request)
      .pipe(tap(response => this.handleAuthentication(response)));
  }

  register(request: RegisterRequest): Observable<Authresponse> {
    return this.http.post<Authresponse>(`${this.apiUrl}/register`, request)
      .pipe(tap(response => this.handleAuthentication(response)));
  }

  googleLogin(token: string): Observable<Authresponse> {
    return this.http.post<Authresponse>(`${this.apiUrl}/google-login`, { token })
      .pipe(tap(response => this.handleAuthentication(response)));
  }

  private handleAuthentication(response: Authresponse) {
    localStorage.setItem('token', response.token);
    this.tokenSubjet.next(response.token);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.tokenSubjet.next(null);
    localStorage.removeItem('user');
  }

  getToken(): string | null {
    return this.tokenSubjet.value;
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}