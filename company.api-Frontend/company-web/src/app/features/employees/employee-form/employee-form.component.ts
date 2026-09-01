import { Component, inject, linkedSignal, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, FormRoot } from '@angular/forms/signals';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EmployeeService } from '../../../core/services/employee.service';
import { DepartmentsService } from '../../../core/services/departments.service';
import { EmployeeFormModel, mapFormToCreateRequest, mapFormToUpdateRequest } from '../../../core/models/employee.model';
import { department } from '../../../core/models/departments.model';
import { employeeSchema } from './employee-schema';

const EMPTY_EMPLOYEE: EmployeeFormModel = {
    fullName: '', nationalId: '', departmentId: '', position: '', email: '', phone: '', status: 'Active'
};
@Component({
    selector: 'app-employee-form',
    standalone: true,
    imports: [CommonModule, FormField, FormRoot, RouterLink],
    templateUrl: './employee-form.component.html'
})
export class EmployeeFormComponent implements OnInit {
    private employeeService = inject(EmployeeService);
    private departmentsService = inject(DepartmentsService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    employeeId: number | null = null;
    protected readonly String = String;
    protected readonly departments = signal<department[]>([]);
    protected readonly model = signal<EmployeeFormModel>(EMPTY_EMPLOYEE);
    // The employee form, wired to the model signal and the schema above.
    // The `submission` block replaces a manual (click) handler.
    protected readonly employeeForm = form(this.model, employeeSchema, {
        submission: {
            action: async (f) => this.save(f().value()),
            onInvalid: () => console.warn('Form is invalid — fix the highlighted fields.')
        }
    });
    ngOnInit(): void {
        this.departmentsService.getALL().subscribe(deps => {
            this.departments.set(deps);
        });
        const idParam = this.route.snapshot.paramMap.get('id');
        if (idParam) {
            this.employeeId = +idParam;
            this.employeeService.getById(this.employeeId).subscribe((emp: any) => {
                this.model.set({ 
                    ...EMPTY_EMPLOYEE, 
                    ...emp,
                    departmentId: emp.departmentId ? String(emp.departmentId) : ''
                });
            });
        }
    }
    private async save(value: EmployeeFormModel): Promise<void> {
        if (this.employeeId) {
            await this.employeeService.update(this.employeeId, mapFormToUpdateRequest(value)).toPromise();
        } else {
            await this.employeeService.create(mapFormToCreateRequest(value)).toPromise();
        }
        this.router.navigate(['/employees']);
    }
}