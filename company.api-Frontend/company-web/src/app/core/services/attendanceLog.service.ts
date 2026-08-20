import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AttendanceLog } from '../models/attendanceLog.model';

@Injectable({ providedIn: 'root' })
export class AttendanceLogService {
	private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/attendanceLogs`;
	getAll(employeeId: number, from?: string, to?: string): Observable<AttendanceLog[]> {
		const params: Record<string, string> = { employeeId: employeeId.toString() };
		if (from) params['from'] = from;
		if (to) params['to'] = to;
		return this.http.get<AttendanceLog[]>(this.baseUrl, { params });
	}
}
