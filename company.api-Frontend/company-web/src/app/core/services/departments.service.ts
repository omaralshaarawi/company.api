import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { department } from '../models/departments.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DepartmentsService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/departments`;
    getALL(): Observable<department[]> {
        return this.http.get<department[]>(this.baseUrl);
    }
    getById(id: number): Observable<department> {
        return this.http.get<department>(`${this.baseUrl}/${id}`);
    }
    create(department: department): Observable<department> {
        return this.http.post<department>(this.baseUrl, department);
    }
    update(id: number, department: department): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, department);
    }
    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}