import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DepartmentsService } from '../../../core/services/departments.service';
import { department } from '../../../core/models/departments.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-department-list.component',
  imports: [CommonModule, RouterLink],
  templateUrl: './department-list.component.html',
})
export class DepartmentListComponent {
  private departmentService = inject(DepartmentsService);
  private authService = inject(AuthService);
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
        this.error.set('Could not load departments.');
        this.loading.set(false);
      }
    });
  }
  logout(): void {
    this.authService.logout();
  }
  delete(id: number): void {
    if (!confirm('Delete this department?')) return;

    this.departmentService.delete(id).subscribe({
      next: () => {
        this.departments.update(list => list.filter(d => d.departmentId !== id));
        this.error.set(null);
      },
      error: () => {
        this.error.set('Cannot delete this department because it has active employees.');
      }
    });
  }
}
