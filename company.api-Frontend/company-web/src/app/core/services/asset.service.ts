import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { asset } from '../models/assets.model';
import { environment } from '../../environments/environment';
import { EmployeeAsset } from '../models/employeeAsset.model';
@Injectable({
  providedIn: 'root',
})
export class AssetService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/Assets`;
    getALL(status?: string, assetTypeId?: number): Observable<asset[]> {
        let params: Record<string, string> = {};
        if (status) params['status'] = status;
        if (assetTypeId) params['assetTypeId'] = assetTypeId.toString();
        return this.http.get<asset[]>(this.baseUrl, { params });
    }
    getById(id: number): Observable<asset> {
        return this.http.get<asset>(`${this.baseUrl}/${id}`);
    }
    create(req: asset): Observable<asset> {
        return this.http.post<asset>(this.baseUrl, req);
    }   
    update(id: number, req: asset): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, req);
    }
    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
    getAssetHistory(id: number): Observable<EmployeeAsset[]> {
        return this.http.get<EmployeeAsset[]>(`${this.baseUrl}/${id}/history`);
    }
}