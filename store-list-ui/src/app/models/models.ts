export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface Authresponse {
  token: string;
}

export interface ShoppingListItem {
  id: string;
  name: string;
  quantity: number;
  isChecked: boolean;
  shoppingListId?: string;
}

export interface ShoppingList {
  id: string;
  name: string;
  items: ShoppingListItem[];
  userId: User['id'] | undefined;
}

export interface User {
  id: string;
  userName: string;
  email: string;
  firstName: string;
  lastName: string;
}