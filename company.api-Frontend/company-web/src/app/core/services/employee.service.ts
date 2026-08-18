import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { Employee, CreateEmployeeRequest, UpdateEmployeeRequest, PagedEmployeesResponse } from '../models/employee.model';
@Injectable({ providedIn: 'root' })
export class EmployeeService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/employees`;
    getAll(departmentId?: number, status?: string): Observable<Employee[]> {
        let params: Record<string, string> = {};
        if (departmentId) params['departmentId'] = departmentId.toString();
        if (status) params['status'] = status;
        return this.http.get<Employee[] | PagedEmployeesResponse>(this.baseUrl, { params }).pipe(
            map((response) => {
                if (Array.isArray(response)) {
                    return response;
                }
                return response?.items ?? [];
            })
        );
    }
    getById(id: number): Observable<Employee> {
        return this.http.get<Employee>(`${this.baseUrl}/${id}`);
    }
    create(req: CreateEmployeeRequest): Observable<Employee> {
        return this.http.post<Employee>(this.baseUrl, req);
    }
    update(id: number, req: UpdateEmployeeRequest): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, req);
    }
    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}