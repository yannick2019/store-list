import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
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

  updateList(id: string, list: ShoppingList): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, list);
  }

  deleteList(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  updateItemCheckState(itemId: string, isChecked: boolean): Observable<any> {
    return this.http.patch(`${this.apiUrl}/items/${itemId}/check`, isChecked);
  }
}
