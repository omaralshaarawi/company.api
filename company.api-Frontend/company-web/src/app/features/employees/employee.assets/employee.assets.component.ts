import { Component, inject, signal, OnInit,effect,computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { EmployeeAssetService } from '../../../core/services/employeeAsset.service';
import { AssetService } from '../../../core/services/asset.service';
import { EmployeeAsset } from '../../../core/models/employeeAsset.model';
import { asset } from '../../../core/models/assets.model';
import { NotificationService } from '../../../core/services/notification.service'
import * as signalR from '@microsoft/signalr';
@Component({
  selector: 'app-employee.assets.component',
  imports: [CommonModule, RouterLink],
  templateUrl: './employee.assets.component.html',
  styleUrl: './employee.assets.component.scss',
})
export class EmployeeAssetsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private employeeAssetService = inject(EmployeeAssetService);
  private assetService = inject(AssetService);
  protected notificationService = inject(NotificationService);

  employeeId = 0;
  employeesAssets = signal<EmployeeAsset[]>([]);
  assets = signal<asset[]>([]);
    loading = signal(true);
    error = signal<string | null>(null);
    returnError = signal<string | null>(null);
    returningId = signal<number | null>(null);
    protected readonly liveStatus = computed(() => {
    switch (this.notificationService.connectionState()) {
      case signalR.HubConnectionState.Connected:
        return { label: 'Live', cssClass: 'status-live' };
      case signalR.HubConnectionState.Reconnecting:
        return { label: 'Reconnecting…', cssClass: 'status-reconnecting' };
      case signalR.HubConnectionState.Connecting:
        return { label: 'Connecting…', cssClass: 'status-reconnecting' };
      default:
        return { label: 'Offline', cssClass: 'status-offline' };
    }
  });


    constructor() {
        // Runs automatically whenever lastAssetAssigned changes
        effect(() => {
            const event = this.notificationService.lastAssetAssigned();
            if (event) {
                this.employeeAssetService.getAll().subscribe(data => this.employeesAssets.set(data));
            }
        });

        effect(() => {
            const event = this.notificationService.lastAssetReturned();
            if (event) {
                this.employeeAssetService.getAll().subscribe(data => this.employeesAssets.set(data));
            }
        });
    }
    ngOnInit(): void {
    this.employeeId = Number(this.route.snapshot.paramMap.get('id'));
    this.employeeAssetService.getAll(this.employeeId).subscribe({
      next: (employeesAssets) => {
        this.employeesAssets.set(employeesAssets ?? []);
        this.loadAssets();
      },
      error: () => {
        this.error.set('Could not load this employee\'s assets.');
        this.loading.set(false);
      }
        });
    }

  private loadAssets(): void {
    this.assetService.getALL().subscribe({
      next: (assets) => {
        this.assets.set(assets);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load asset details.');
        this.loading.set(false);
      }
    });
  }

  getAsset(assetId: number): asset | undefined {
    return this.assets().find(item => item.assetId === assetId);
  }

  returnAsset(assignment: EmployeeAsset): void {
    if (!confirm('Return this asset?')) return;

    this.returningId.set(assignment.employeeAssetId);
    this.returnError.set(null);
    this.employeeAssetService.returnAsset(assignment.employeeAssetId).subscribe({
      next: (updatedAssignment) => {
        this.employeesAssets.update(assignments => assignments.map(item =>
          item.employeeAssetId === updatedAssignment.employeeAssetId ? updatedAssignment : item
        ));
        this.returningId.set(null);
      },
      error: () => {
        this.returningId.set(null);
        this.returnError.set('This asset could not be returned. It may already have been returned or changed by another user.');
      }
    });
  }
}
