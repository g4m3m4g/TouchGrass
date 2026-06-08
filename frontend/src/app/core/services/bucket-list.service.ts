import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  BucketItemResponse,
  CreateBucketItemRequest,
  UpdateBucketItemRequest,
} from '../../shared/models/bucket-item.model';

@Injectable({
  providedIn: 'root',
})
export class BucketListService {
  private apiUrl = 'http://localhost:5266/api/BucketList';

  constructor(private http: HttpClient) {}

  // Helper สำหรับสร้าง Header แนบ Token (ดึงจาก LocalStorage)
  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('jwtToken') || '';
    return new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });
  }

  getItems(): Observable<BucketItemResponse[]> {
    return this.http.get<BucketItemResponse[]>(this.apiUrl, { headers: this.getHeaders() });
  }

  createItem(item: CreateBucketItemRequest): Observable<BucketItemResponse> {
    return this.http.post<BucketItemResponse>(this.apiUrl, item, { headers: this.getHeaders() });
  }

  updateItem(id: number, item: UpdateBucketItemRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, item, { headers: this.getHeaders() });
  }

  deleteItem(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }

  pickRandom(): Observable<BucketItemResponse> {
    return this.http.get<BucketItemResponse>(`${this.apiUrl}/pick-random`, {
      headers: this.getHeaders(),
    });
  }
}
