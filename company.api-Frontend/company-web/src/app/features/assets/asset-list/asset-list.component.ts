import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AssetService } from '../../../core/services/asset.service';
import { asset } from '../../../core/models/assets.model';
import { AuthService } from '../../../core/services/auth.service';
import { AssetTypeService } from '../../../core/services/assetType.service';
import { assetType } from '../../../core/models/assetTypes.model';

@Component({
  selector: 'app-asset-list.component',
  imports: [CommonModule, RouterLink],
  templateUrl: './asset-list.component.html',
  styleUrl: './asset-list.component.scss',
})
export class AssetListComponent implements OnInit {
  private assetService = inject(AssetService);
  private assetTypeService = inject(AssetTypeService);
  private authService = inject(AuthService);
  error = signal<string | null>(null);
  loading = signal(true);
  assets = signal<asset[]>([]);
  assetTypes = signal<assetType[]>([]);
  selectedStatus?: string;
  selectedAssetTypeId?: number;

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
        this.error.set('Could not load assets.');
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

  delete(id: number): void {
    if (!confirm('Delete this asset?')) return;
    this.assetService.delete(id).subscribe(() => {
      this.assets.update(list => list.filter(a => a.assetId !== id));
    });
  }

  logout(): void {
    this.authService.logout();
  }
  
  getAssetHistory(id:number): void{
    this.assetService.getAssetHistory(id);
  }

}
