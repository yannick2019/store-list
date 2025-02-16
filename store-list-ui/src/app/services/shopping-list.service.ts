import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

interface ShoppingListItem {
  id: string;
  name: string;
  quantity: number;
  isChecked: boolean;
}

interface ShoppingList {
  id: string;
  name: string;
  items: ShoppingListItem[];
}

@Injectable({
  providedIn: 'root'
})
export class ShoppingListService {
  private apiUrl = 'https://localhost:7019/api/shoppinglist';

  private http = inject(HttpClient);

  getAllLists(): Observable<ShoppingList[]> {
    return this.http.get<ShoppingList[]>(this.apiUrl);
  }

  getListById(id: string): Observable<ShoppingList> {
    return this.http.get<ShoppingList>(`${this.apiUrl}/${id}`);
  }

  addList(list: ShoppingList): Observable<any> {
    return this.http.post(this.apiUrl, list);
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
