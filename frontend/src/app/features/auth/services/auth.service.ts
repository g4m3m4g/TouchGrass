// src/app/features/auth/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5266/api/Auth';

  constructor(private http: HttpClient) {}

  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        if (response && response.token) {
          // เก็บ Token ลง LocalStorage ทันทีเมื่อล็อกอินผ่าน
          localStorage.setItem('jwtToken', response.token);
        }
      })
    );
  }

  register(userData: { username: string; email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, userData);
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('jwtToken');
  }
}