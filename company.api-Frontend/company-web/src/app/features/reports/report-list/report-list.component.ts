import { Component, computed, inject, OnInit, signal,effect } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReportService } from '../../../core/services/report.service';
import { createReport, report } from '../../../core/models/reports.model';
import { EmployeeService } from '../../../core/services/employee.service';
import { Employee } from '../../../core/models/employee.model';
import { AssetService } from '../../../core/services/asset.service';
import { asset } from '../../../core/models/assets.model';
import { NotificationService } from '../../../core/services/notification.service'

@Component({
  selector: 'app-report-list.component',
  imports: [CommonModule, RouterLink, DatePipe],
  templateUrl: './report-list.component.html',
  styleUrl: './report-list.component.scss',
})
export class ReportListComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly reportService = inject(ReportService);
  private readonly employeeService = inject(EmployeeService);
  private readonly assetService = inject(AssetService);
  private notificationService = inject(NotificationService);

  protected readonly employeeId = signal<number | null>(null);
  protected readonly assetId = signal<number | null>(null);
  protected readonly reportTypeId = signal<number | null>(null);
  protected readonly reports = signal<report[]>([]);
  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);

  protected readonly search = signal('');
  protected readonly selectedReportType = signal<string>('');
  protected readonly selectedEmployeeName = signal<string>('');
  protected readonly selectedAssetName = signal<string>('');
  protected readonly employees = signal<Employee[]>([]);
  protected readonly assets = signal<asset[]>([]);

  protected readonly showGenerateModal = signal(false);
  protected readonly generateType = signal<string>('Attendance Summary');
  protected readonly generateEmployeeId = signal<number | null>(null);

  protected readonly reportTypes = ['Attendance Summary', 'Asset Audit', 'Employee Activity'];

  protected readonly filteredReports = computed(() => {
    const searchTerm = this.search().trim().toLowerCase();

    return this.reports().filter(item => {
      const matchesSearch = !searchTerm || item.title.toLowerCase().includes(searchTerm);
      const matchesType = !this.selectedReportType() || item.reportTypeName === this.selectedReportType();
      const matchesEmployee = !this.selectedEmployeeName() || item.employeeName === this.selectedEmployeeName();
      return matchesSearch && matchesType && matchesEmployee;
    });
  });


  constructor() {
        effect(() => {
            const event = this.notificationService.lastReportGenerated();
            if (event) {
                this.reportService.getAll().subscribe(data => this.reports.set(data));
            }
        });

    }

  ngOnInit(): void {
    const employeeFromRoute = this.route.snapshot.paramMap.get('id');
    if (employeeFromRoute) {
      this.employeeId.set(Number(employeeFromRoute));
    }

    this.loadEmployees();
    this.loadAssets();
    this.loadReports();
  }

  protected loadEmployees(): void {
    this.employeeService.getAll().subscribe({
      next: employees => this.employees.set(employees),
      error: () => this.employees.set([])
    });
  }

  protected loadAssets(): void {
    this.assetService.getALL().subscribe({
      next: assets => this.assets.set(assets),
      error: () => this.assets.set([])
    });
  }

  protected loadReports(): void {
    this.loading.set(true);
    this.error.set(null);

    const employeeId = this.selectedEmployeeName()
      ? this.employees().find(employee => employee.fullName === this.selectedEmployeeName())?.employeeId
      : this.employeeId() ?? undefined;

    const assetId = this.selectedAssetName()
      ? this.assets().find(asset => asset.assetName === this.selectedAssetName())?.assetId
      : this.assetId() ?? undefined;

    const reportTypeId = this.getReportTypeId(this.selectedReportType());

    this.reportService.getAll(employeeId, assetId, reportTypeId).subscribe({
      next: reports => {
        this.reports.set(reports);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load reports.');
        this.loading.set(false);
      }
    });
  }

  protected onReportTypeChange(value: string): void {
    this.selectedReportType.set(value);
    this.loadReports();
  }

  protected onEmployeeFilterChange(value: string): void {
    this.selectedEmployeeName.set(value);
    this.loadReports();
  }

  protected onAssetFilterChange(value: string): void {
    this.selectedAssetName.set(value);
    this.loadReports();
  }

  protected openGenerateModal(): void {
    this.showGenerateModal.set(true);
  }

  protected closeGenerateModal(): void {
    this.showGenerateModal.set(false);
    this.generateType.set('Attendance Summary');
    this.generateEmployeeId.set(null);
  }

  protected onGenerateTypeChange(value: string): void {
    this.generateType.set(value);

    if (value !== 'Employee Activity') {
      this.generateEmployeeId.set(null);
    }
  }

  protected generateReport(): void {
    const selectedType = this.generateType();
    const typeId = this.getReportTypeId(selectedType) ?? null;

    const request: createReport = {
      reportTypeId: typeId,
      title: selectedType,
      generatedById: null,
      relatedEmployeeId: selectedType === 'Employee Activity' ? this.generateEmployeeId() ?? null : null,
      relatedAssetId: null,
    };

    this.reportService.create(request).subscribe({
      next: () => {
        this.closeGenerateModal();
        this.loadReports();
      },
      error: () => {
        this.error.set('Could not generate report.');
      }
    });
  }

  protected getReportTypeId(typeName: string): number | undefined {
    switch (typeName) {
      case 'Attendance Summary':
        return 1003;
      case 'Asset Audit':
        return 1004;
      case 'Employee Activity':
        return 1005;
      default:
        return undefined;
    }
  }

  protected reportTypeColor(type: string | null): string {
    switch ((type ?? '').toLowerCase()) {
      case 'attendance summary':
        return 'type-attendance';
      case 'asset audit':
        return 'type-asset';
      case 'employee activity':
        return 'type-employee';
      default:
        return 'type-default';
    }
  }
}
