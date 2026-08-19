import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CreateEmployeeAssetRequest, EmployeeAsset } from '../models/employeeAsset.model';
@Injectable({
  providedIn: 'root',
})
export class EmployeeAssetService{
        private http = inject(HttpClient);
        private baseUrl = `${environment.apiUrl}/EmployeeAssets`;
        getAll(employeeId?: number, assetId?: number, active?: boolean): Observable<EmployeeAsset[]> {
            const params: Record<string, string> = {};
            if (employeeId !== undefined) params['employeeId'] = employeeId.toString();
            if (assetId !== undefined) params['assetId'] = assetId.toString();
            if (active !== undefined) params['active'] = active.toString();
            return this.http.get<EmployeeAsset[]>(this.baseUrl, { params });
        }
        getById(id: number): Observable<EmployeeAsset> {
            return this.http.get<EmployeeAsset>(`${this.baseUrl}/${id}`);
        }
        create(req: CreateEmployeeAssetRequest): Observable<EmployeeAsset> {
            return this.http.post<EmployeeAsset>(this.baseUrl, req);
        }   
        returnAsset(id: number, notes: string | null = null): Observable<EmployeeAsset> {
            return this.http.put<EmployeeAsset>(`${this.baseUrl}/${id}/return`, notes);
        }
        update(id: number, req: EmployeeAsset): Observable<void> {
            return this.http.put<void>(`${this.baseUrl}/${id}`, req);
        }
        delete(id: number): Observable<void> {
            return this.http.delete<void>(`${this.baseUrl}/${id}`);
        }
}