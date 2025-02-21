import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Authresponse, LoginRequest, RegisterRequest, User } from '../models/models';
import { UserStateService } from './user-state.service';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7019/api/account';
  private http = inject(HttpClient);
  private tokenSubject = new BehaviorSubject<string | null>(localStorage.getItem('token'));
  private userState = inject(UserStateService);

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
    this.tokenSubject.next(response.token);

    const decodedToken = jwtDecode<any>(response.token);
    const user: User = {
      userName: decodedToken.unique_name,
      email: decodedToken.email,
      firstName: decodedToken.firstName,
      lastName: decodedToken.lastName,
    };
    this.userState.setUser(user);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.tokenSubject.next(null);
    this.userState.setUser(null);
  }

  getToken(): string | null {
    return this.tokenSubject.value;
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}