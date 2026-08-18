import { Component, inject, linkedSignal, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, FormRoot } from '@angular/forms/signals';
import { AssetTypeService } from '../../../core/services/assetType.service';
import { assetType } from '../../../core/models/assetTypes.model';
import { ActivatedRoute, Router } from '@angular/router';
import { assetTypeSchema } from './asset-type-schema';
import { RouterLink } from '@angular/router';

const EMPTY_ASSET_TYPE: assetType = { assetTypeId: 0, typeName: '' };
@Component({
  selector: 'app-asset-type-form.component',
  imports: [CommonModule, FormRoot, FormField,RouterLink],
  templateUrl: './asset-type-form.component.html',
})
export class AssetTypeFormComponent implements OnInit {
  private assetTypeService = inject(AssetTypeService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  assetTypeId: number | null = null;
  protected readonly model = signal<assetType>(EMPTY_ASSET_TYPE );
  protected readonly assetTypeForm = form(this.model, assetTypeSchema, {
    submission: {
      action: async (f) => this.save(f().value()),
      onInvalid: () => console.warn('Form is invalid — fix the highlighted fields.')
    }
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.assetTypeId = +idParam;
      this.assetTypeService.getById(this.assetTypeId).subscribe(dep => {
        this.model.set(dep);
      });
    }
  }

  private async save(value: assetType): Promise<void> {
    if (this.assetTypeId) {
      await this.assetTypeService.update(this.assetTypeId, value.typeName).toPromise();
    } else {
      await this.assetTypeService.create(value.typeName).toPromise();
    }
    this.router.navigate(['/asset-types']);
  }
}
