import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AssetTypeService } from '../../../core/services/assetType.service';
import { assetType } from '../../../core/models/assetTypes.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-asset-type-list.component',
  imports: [CommonModule, RouterLink],
  templateUrl: './asset-type-list.component.html',
})
export class AssetTypeListComponent implements OnInit {
  private assetTypeService = inject(AssetTypeService);
  private authService = inject(AuthService);
  assetTypes = signal<assetType[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.assetTypeService.getALL().subscribe({
      next: (data) => {
        this.assetTypes.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Could not load asset types.');
        this.loading.set(false);
      }
    });
  }

  logout(): void {
    this.authService.logout();
  }

  delete(id: number): void {
    if (!confirm('Delete this asset type?')) return;

    this.assetTypeService.delete(id).subscribe({
      next: () => {
        this.assetTypes.update(list => list.filter(a => a.assetTypeId !== id));
        this.error.set(null);
      },
      error: () => {
        this.error.set('Cannot delete this asset type because it is in use.');
      }
    });
  }

}
