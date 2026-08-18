import { Component, inject, linkedSignal, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, FormRoot } from '@angular/forms/signals';
import { AssetService } from '../../../core/services/asset.service';
import { asset } from '../../../core/models/assets.model';
import { ActivatedRoute, Router } from '@angular/router';
import { assetSchema } from './assets-schema';
import { RouterLink } from '@angular/router';

const EMPTY_ASSET: asset = {
  assetName: '',
  assetTypeId: null,
  assetId: 0,
  serialNumber: '',
  purchaseDate: '',
  purchaseCost: 0,
  status: ''
};

@Component({
  selector: 'app-asset-form.component',
  imports: [CommonModule, FormRoot, FormField, RouterLink],
  templateUrl: './asset-form.component.html',
  styleUrl: './asset-form.component.scss',
})
export class AssetFormComponent {
  private assetService = inject(AssetService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    assetId: number | null = null;
    protected readonly model = signal<asset>(EMPTY_ASSET);
    protected readonly assetForm = form(this.model, assetSchema, {
      submission: {
        action: async (f) => this.save(f().value()),
        onInvalid: () => console.warn('Form is invalid — fix the highlighted fields.')
      }
    });
  
    ngOnInit(): void {
      const idParam = this.route.snapshot.paramMap.get('id');
      if (idParam) {
        this.assetId = +idParam;
        this.assetService.getById(this.assetId).subscribe(dep => {
          this.model.set(dep);
        });
      }
    }
  
    private async save(value: asset): Promise<void> {
      if (this.assetId) {
        await this.assetService.update(this.assetId, value).toPromise();
      } else {
        await this.assetService.create(value).toPromise();
      }
      this.router.navigate(['/assets']);
    }
}
