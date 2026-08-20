// features/employees/employee-form/employee-form-reactive.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../../../core/services/employee.service';
@Component({
    selector: 'app-employee-form-reactive',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './employee-form-reactive.component.html'
})
export class EmployeeFormReactiveComponent implements OnInit {
    private fb = inject(FormBuilder);
    private employeeService = inject(EmployeeService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    employeeId: number | null = null;
    form = this.fb.group({
        fullName: ['', [Validators.required, Validators.maxLength(150)]],
        departmentId: [null as number | null],
        position: [''],
        email: ['', [Validators.email]],
        phone: [''],
        status: ['Active']
    });
    ngOnInit(): void {
        const idParam = this.route.snapshot.paramMap.get('id');
        if (idParam) {
            this.employeeId = +idParam;
            this.employeeService.getById(this.employeeId).subscribe(emp => {
                this.form.patchValue(emp);
            });
        }
    }
    submit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }
        const value = this.form.getRawValue();
        if (this.employeeId) {
            this.employeeService.update(this.employeeId, value as any).subscribe(
                () => this.router.navigate(['/employees'])
            );
        } else {
            this.employeeService.create(value as any).subscribe(
                () => this.router.navigate(['/employees'])
            );
        }
    }
}