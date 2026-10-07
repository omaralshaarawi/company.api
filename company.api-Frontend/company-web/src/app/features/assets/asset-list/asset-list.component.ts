import { Component, computed, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AssetService } from '../../../core/services/asset.service';
import { asset } from '../../../core/models/assets.model';
import { AssetTypeService } from '../../../core/services/assetType.service';
import { assetType } from '../../../core/models/assetTypes.model';
import { NotificationService } from '../../../core/services/notification.service';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-asset-list.component',
  imports: [CommonModule, RouterLink],
  templateUrl: './asset-list.component.html',
  styleUrl: './asset-list.component.scss',
})
export class AssetListComponent implements OnInit {
  private assetService = inject(AssetService);
  private assetTypeService = inject(AssetTypeService);
  private notificationService = inject(NotificationService);
  error = signal<string | null>(null);
  loading = signal(true);
  assets = signal<asset[]>([]);
  assetTypes = signal<assetType[]>([]);
  selectedStatus?: string;
  selectedAssetTypeId?: number;
  protected readonly liveStatus = computed(() => {
    switch (this.notificationService.connectionState()) {
      case signalR.HubConnectionState.Connected:
        return { label: $localize`Live`, cssClass: 'status-live' };
      case signalR.HubConnectionState.Reconnecting:
        return { label: $localize`Reconnecting...`, cssClass: 'status-reconnecting' };
      case signalR.HubConnectionState.Connecting:
        return { label: $localize`Connecting...`, cssClass: 'status-reconnecting' };
      default:
        return { label: $localize`Offline`, cssClass: 'status-offline' };
    }
  });

  ngOnInit(): void {
    this.loadAssetTypes();
    this.loadAssets();
  }

  private loadAssetTypes(): void {
    this.assetTypeService.getALL().subscribe({
      next: (data) => this.assetTypes.set(data),
      error: () => this.assetTypes.set([]),
    });
  }

  loadAssets(): void {
    this.loading.set(true);

    this.assetService.getALL(this.selectedStatus, this.selectedAssetTypeId).subscribe({
      next: (data) => {
        this.assets.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set($localize`Could not load assets.`);
        this.loading.set(false);
      }
    });
  }

  applyFilters(newStatus?: string, newAssetTypeId?: number): void {
    this.selectedStatus = newStatus && newStatus !== '' ? newStatus : undefined;
    this.selectedAssetTypeId = newAssetTypeId && newAssetTypeId > 0 ? newAssetTypeId : undefined;
    this.loadAssets();
  }

  getStatusClass(status: string | null): string {
    switch (status) {
      case 'InStock':
        return 'status-inStock';
      case 'Assigned':
        return 'status-assigned';
      case 'Maintenance':
        return 'status-maintenance';
      case 'Retired':
        return 'status-retired';
      default:
        return 'status-default';
    }
  }

  getStatusLabel(status: string | null): string {
    switch (status) {
      case 'InStock':
        return $localize`In stock`;
      case 'Assigned':
        return $localize`Assigned`;
      case 'Maintenance':
        return $localize`Maintenance`;
      case 'Retired':
        return $localize`Retired`;
      default:
        return status ?? '';
    }
  }

  delete(id: number): void {
    if (!confirm($localize`Delete this asset?`)) return;
    this.assetService.delete(id).subscribe(() => {
      this.assets.update(list => list.filter(a => a.assetId !== id));
    });
  }

  getAssetHistory(id:number): void{
    this.assetService.getAssetHistory(id);
  }

}
