import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AttendanceLogService } from '../../../core/services/attendanceLog.service';
import { AttendanceLog } from '../../../core/models/attendanceLog.model';

@Component({
  selector: 'app-attendancelog-list.component',
  imports: [CommonModule, RouterLink],
  templateUrl: './attendancelog-list.component.html',
  styleUrl: './attendancelog-list.component.scss',
})
export class AttendancelogListComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly attendanceLogService = inject(AttendanceLogService);
  protected readonly employeeId = Number(this.route.snapshot.paramMap.get('id'));
  protected readonly logs = signal<AttendanceLog[]>([]);
  protected readonly from = signal('');
  protected readonly to = signal('');
  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadLogs();
  }

  protected loadLogs(): void {
    this.loading.set(true);
    this.error.set(null);
    this.attendanceLogService.getAll(this.employeeId, this.from(), this.to()).subscribe({
      next: logs => {
        this.logs.set(logs);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load attendance logs.');
        this.loading.set(false);
      }
    });
  }

}
