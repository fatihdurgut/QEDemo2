export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: UserRole;
}

export interface LoginResponse {
  token: string;
  userId: string;
  email: string;
  role: UserRole;
  expiresAt: Date;
}

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
}

export enum UserRole {
  Author = 'Author',
  Publisher = 'Publisher',
  Employee = 'Employee',
  Admin = 'Admin'
}
