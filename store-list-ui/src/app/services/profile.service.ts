import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/models';
import { environment } from '../../environment';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private apiUrl = environment.apiUrl + '/profile';
  private http = inject(HttpClient);

  getProfile(): Observable<User> {
    return this.http.get<User>(this.apiUrl);
  }
}
