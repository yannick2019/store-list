import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, retry, throwError } from 'rxjs';
import { ShoppingList } from '../models/models';
import { environment } from '../../environment';

@Injectable({
  providedIn: 'root'
})
export class ShoppingListService {
  private apiUrl = environment.apiUrl + '/shoppinglist';

  private http = inject(HttpClient);

  getAllLists(): Observable<ShoppingList[]> {
    return this.http.get<ShoppingList[]>(this.apiUrl);
  }

  getListById(id: string): Observable<ShoppingList> {
    return this.http.get<ShoppingList>(`${this.apiUrl}/${id}`);
  }

  addList(list: Omit<ShoppingList, 'id' | 'userId'>): Observable<ShoppingList> {
    return this.http.post<ShoppingList>(this.apiUrl, list);
  }

  updateList(id: string, list: any): Observable<any> {
    //console.log('Sending update data:', JSON.stringify(list));
    
    // Add error handling and retry logic
    return this.http.put(`${this.apiUrl}/${id}`, list)
      .pipe(
        retry(1), // Retry once if connection fails
        catchError(error => {
          if (error.status === 0) {
            console.error('Connection error. Is the server running?');
          }
          return throwError(() => error);
        })
      );
  }

  deleteList(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  updateItemCheckState(itemId: string, isChecked: boolean): Observable<any> {
    return this.http.patch(`${this.apiUrl}/items/${itemId}/check`, isChecked);
  }
}
