import { Component, inject, signal, OnInit, computed,effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EmployeeService } from '../../../core/services/employee.service';
import { AuthService } from '../../../core/services/auth.service';
import { Employee } from '../../../core/models/employee.model';
import { fingerprint, createFingerprintRequest } from '../../../core/models/fingerprints.model';
import { FingerprintsService } from '../../../core/services/fingerpints.serivce';
@Component({
    selector: 'app-employee-list',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './employee-list.component.html'
})
export class EmployeeListComponent implements OnInit {
    private employeeService = inject(EmployeeService);
    private authService = inject(AuthService);
    private fingerprintService = inject(FingerprintsService);
    fingerprints = signal<fingerprint[]>([]);
    employees = signal<Employee[]>([]);
    loading = signal(true);
    error = signal<string | null>(null);
    loadingFingerprints = signal(true);
    errorFingerprints = signal<string | null>(null);
    
    employeeDictionary = computed(() => {
        const map = new Map<number, string>();
        for (const emp of this.employees()) {
            map.set(emp.employeeId, emp.fullName);
        }
        return map;
    });
    ngOnInit(): void {
        this.employeeService.getAll().subscribe({
            next: (data) => { this.employees.set(data); this.loading.set(false); },
            error: (err) => { this.error.set('Could not load employees.'); this.loading.set(false); }
        });
        this.fingerprintService.getAll().subscribe({
            next: (data) => { this.fingerprints.set(data); this.loadingFingerprints.set(false); },
            error: (err) => { this.errorFingerprints.set('Could not load fingeprints.'); this.loadingFingerprints.set(false); }
        });
    }
    logout(): void {
        this.authService.logout();
    }
    delete(id: number): void {
        if (!confirm('Delete this employee?')) return;
        this.employeeService.delete(id).subscribe(() => {
            this.employees.update(list => list.filter(e => e.employeeId !== id));
        });
    }
}