import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EmployeeService } from '../../../core/services/employee.service';
import { Employee } from '../../../core/models/employee.model';
@Component({
    selector: 'app-employee-list',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './employee-list.component.html'
})
export class EmployeeListComponent implements OnInit {
    private employeeService = inject(EmployeeService);
    employees = signal<Employee[]>([]);
    loading = signal(true);
    error = signal<string | null>(null);
    ngOnInit(): void {
        this.employeeService.getAll().subscribe({
            next: (data) => { this.employees.set(data); this.loading.set(false); },
            error: (err) => { this.error.set('Could not load employees.'); this.loading.set(false); }
        });
    }
    delete(id: number): void {
        if (!confirm('Delete this employee?')) return;
        this.employeeService.delete(id).subscribe(() => {
            this.employees.update(list => list.filter(e => e.employeeId !== id));
        });
    }
}