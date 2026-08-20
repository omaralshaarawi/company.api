import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { report } from '../models/reports.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ReportService{
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/reports`;
    getAll(employeeId?: number, assetId?: number,reportId?: number): Observable<report[]>{
        const params: Record<string, string> = {};
        if (employeeId !== undefined) params['employeeId'] = employeeId.toString();
        if (assetId !== undefined) params['assetId'] = assetId.toString();
        if(reportId!== undefined)params['reportId'] = reportId.toString();
        return this.http.get<report[]>(this.baseUrl,{params});
    }
    getById(id:number): Observable<report>{
            return this.http.get<report>(`${this.baseUrl}/${id}`);
    }
}