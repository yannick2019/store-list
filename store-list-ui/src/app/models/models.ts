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

export interface User {
  userName: string;
  email: string;
  firstName: string;
  lastName: string;
}