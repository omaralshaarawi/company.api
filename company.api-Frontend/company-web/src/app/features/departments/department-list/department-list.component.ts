import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DepartmentsService } from '../../../core/services/departments.service';
import { department } from '../../../core/models/departments.model';

@Component({
  selector: 'app-department-list.component',
  imports: [CommonModule, RouterLink],
  templateUrl: './department-list.component.html',
})
export class DepartmentListComponent {
  private departmentService = inject(DepartmentsService);
  departments = signal<department[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  ngOnInit(): void {
    this.departmentService.getALL().subscribe({
      next: (data) => {
        this.departments.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set($localize`Could not load departments.`);
        this.loading.set(false);
      }
    });
  }
  delete(id: number): void {
    if (!confirm($localize`Delete this department?`)) return;

    this.departmentService.delete(id).subscribe({
      next: () => {
        this.departments.update(list => list.filter(d => d.departmentId !== id));
        this.error.set(null);
      },
      error: () => {
        this.error.set($localize`Cannot delete this department because it has active employees.`);
      }
    });
  }
}
