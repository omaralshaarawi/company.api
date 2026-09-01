import { Component, inject, linkedSignal, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, FormRoot } from '@angular/forms/signals';
import { DepartmentsService } from '../../../core/services/departments.service';
import { department, CreateDepartmentRequest } from '../../../core/models/departments.model';
import { ActivatedRoute, Router } from '@angular/router';
import { departmentSchema } from './department-schema';
import { RouterLink } from '@angular/router';

const EMPTY_DEPARTMENT: CreateDepartmentRequest = { name: '' };

@Component({
  selector: 'app-department-form.component',
  imports: [CommonModule, FormRoot, FormField, RouterLink],
  templateUrl: './department-form.component.html',
})
export class DepartmentFormComponent {
  private departmentService = inject(DepartmentsService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  departmentId: number | null = null;
  protected readonly model = signal<CreateDepartmentRequest>(EMPTY_DEPARTMENT);
  protected readonly departmentForm = form(this.model, departmentSchema, {
    submission: {
      action: async (f) => this.save(f().value()),
      onInvalid: () => console.warn('Form is invalid — fix the highlighted fields.')
    }
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.departmentId = +idParam;
      this.departmentService.getById(this.departmentId).subscribe(dep => {
        this.model.set({ name: dep.name });
      });
    }
  }

  private async save(value: CreateDepartmentRequest): Promise<void> {
    if (this.departmentId) {
      await this.departmentService.update(this.departmentId, value as any).toPromise();
    } else {
      await this.departmentService.create(value as any).toPromise();
    }
    this.router.navigate(['/departments']);
  }
}
