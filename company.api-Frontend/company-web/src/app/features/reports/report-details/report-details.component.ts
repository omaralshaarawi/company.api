import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { report } from '../../../core/models/reports.model';
import { ReportService } from '../../../core/services/report.service';

@Component({
  selector: 'app-report-details.component',
  imports: [CommonModule, RouterLink, DatePipe],
  templateUrl: './report-details.component.html',
  styleUrl: './report-details.component.scss',
})
export class ReportDetailsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly reportService = inject(ReportService);

  protected readonly report = signal<report | null>(null);
  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.error.set('Report not found.');
      this.loading.set(false);
      return;
    }

    this.reportService.getById(id).subscribe({
      next: reportItem => {
        this.report.set(reportItem);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load report details.');
        this.loading.set(false);
      }
    });
  }
}
