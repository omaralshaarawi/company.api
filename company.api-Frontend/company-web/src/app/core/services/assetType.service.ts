import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { assetType } from '../models/assetTypes.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AssetTypeService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/AssetTypes`;
    private jsonHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

    getALL(): Observable<assetType[]> {
        return this.http.get<assetType[]>(this.baseUrl);
    }
    getById(id: number): Observable<assetType> {
        return this.http.get<assetType>(`${this.baseUrl}/${id}`);
    }
    create(assetTypeName: string): Observable<assetType> {
        return this.http.post<assetType>(this.baseUrl, JSON.stringify(assetTypeName), {
            headers: this.jsonHeaders,
        });
    }
    update(id: number, assetTypeName: string): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, JSON.stringify(assetTypeName), {
            headers: this.jsonHeaders,
        });
    }
    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}