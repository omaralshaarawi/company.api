import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, FormRoot } from '@angular/forms/signals';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AssetService } from '../../../core/services/asset.service';
import { EmployeeAssetService } from '../../../core/services/employeeAsset.service';
import { asset } from '../../../core/models/assets.model';
import { EmployeeAssetFormModel } from '../../../core/models/employeeAsset.model';
import { employeeAssetFormSchema } from './employee.assets-schema';



const EMPTY_EMPLOYEE_ASSET: EmployeeAssetFormModel = {
  assetId: '',
  notes: ''
};

@Component({
  selector: 'app-employee.assets-form.component',
  imports: [CommonModule, FormField, FormRoot, RouterLink],
  templateUrl: './employee.assets-form.component.html',
  styleUrl: './employee.assets-form.component.scss',
})
export class EmployeeAssetsFormComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly assetService = inject(AssetService);
  private readonly employeeAssetService = inject(EmployeeAssetService);
  employeeId = 0;
  availableAssets = signal<asset[]>([]);
  loading = signal(true);
  saving = signal(false);
  error = signal<string | null>(null);
  protected readonly model = signal<EmployeeAssetFormModel>(EMPTY_EMPLOYEE_ASSET);
  protected readonly employeeAssetForm = form(this.model, employeeAssetFormSchema, {
    submission: {
      action: async (formState) => this.save(formState().value()),
      onInvalid: () => console.warn('Form is invalid - select an asset first.')
    }
  });

  ngOnInit(): void {
    this.employeeId = Number(this.route.snapshot.paramMap.get('id'));
    this.assetService.getALL('InStock').subscribe({
      next: assets => {
        this.availableAssets.set(assets);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load available assets.');
        this.loading.set(false);
      }
    });
  }

  private async save(value: EmployeeAssetFormModel): Promise<void> {
    const assetId = Number(value.assetId);
    if (!assetId) return;
    this.saving.set(true);
    this.error.set(null);
    await this.employeeAssetService.create({
      employeeId: this.employeeId,
      assetId,
      assignedDate: new Date().toISOString().slice(0, 10),
      returnDate: null,
      notes: value.notes.trim() || null
    }).toPromise();
    await this.router.navigate(['/employees', this.employeeId, 'assets']);
  }
}
