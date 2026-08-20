import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { fingerprint,createFingerprintRequest } from '../models/fingerprints.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FingerprintsService{
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/fingerprints`;
    getAll(employeeId?: number): Observable<fingerprint[]> {
        const params: Record<string, string> = {};
        if (employeeId !== undefined) params['employeeId'] = employeeId.toString();
        return this.http.get<fingerprint[]>(this.baseUrl, { params });
    }
    getById(id:number): Observable<fingerprint>{
        return this.http.get<fingerprint>(`${this.baseUrl}/${id}`);
    }
    create(req: createFingerprintRequest ) : Observable<fingerprint>{
        return this.http.post<fingerprint>(this.baseUrl,req);
    }
    update(id:number,req:fingerprint): Observable<void>{
        return this.http.put<void>(`${this.baseUrl}/${id}`,req);
    }
    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}