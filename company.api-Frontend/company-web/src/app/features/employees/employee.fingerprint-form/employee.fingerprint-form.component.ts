import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, FormRoot } from '@angular/forms/signals';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FingerprintsService } from '../../../core/services/fingerpints.serivce';
import { createFingerprintRequest } from '../../../core/models/fingerprints.model';
import { employeeFingerprintFormSchema } from './employee.fingerprint-schema';

type FingerprintFormModel = {
  employeeId: number;
  fingerIndex: string;
  deviceId: string;
  enrolledDate: string | null;
  quality: string;
};

const EMPTY_EMPLOYEE_FINGERPRINT: FingerprintFormModel = {
  employeeId: 0,
  fingerIndex: '',
  deviceId: '',
  enrolledDate: null,
  quality: ''
};

@Component({
  selector: 'app-employee.fingerprint-form.component',
  imports: [CommonModule, FormField, FormRoot, RouterLink],
  templateUrl: './employee.fingerprint-form.component.html',
  styleUrl: './employee.fingerprint-form.component.scss',
})
export class EmployeeFingerprintFormComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly fingerprintService = inject(FingerprintsService);
  protected readonly fingerIndexOptions = [
    'LeftThumb',
    'LeftIndex',
    'LeftMiddle',
    'LeftRing',
    'LeftLittle',
    'RightThumb',
    'RightIndex',
    'RightMiddle',
    'RightRing',
    'RightLittle'
  ];
  protected readonly employeeId = Number(this.route.snapshot.paramMap.get('id'));
  protected readonly saving = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly model = signal<FingerprintFormModel>(EMPTY_EMPLOYEE_FINGERPRINT);
  protected readonly employeeFingerprintForm = form(this.model, employeeFingerprintFormSchema, {
    submission: {
      action: async (f) => this.save(f().value()),
      onInvalid: () => this.error.set('Form is invalid. Please fix the highlighted fields.')
    }
  });

  ngOnInit(): void {
    this.model.set({ ...EMPTY_EMPLOYEE_FINGERPRINT, employeeId: this.employeeId });
  }

  private async save(value: FingerprintFormModel): Promise<void> {
    this.saving.set(true);
    this.error.set(null);
    try {
      await this.fingerprintService.create({
        ...value,
        employeeId: this.employeeId,
        quality: value.quality.trim() || null,
        templateData: '' 
      }).toPromise();
      await this.router.navigate(['/employees']);
    } catch {
      this.saving.set(false);
      this.error.set('The fingerprint could not be saved. Please try again.');
    }
  }
}
