import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AssetService } from '../../../core/services/asset.service';
import { EmployeeAsset } from '../../../core/models/employeeAsset.model';

@Component({
	selector: 'app-asset-history',
	standalone: true,
	imports: [CommonModule, RouterLink],
	templateUrl: './asset-history.component.html',
	styleUrl: './asset-history.component.scss'
})
export class AssetHistoryComponent implements OnInit {
	private readonly route = inject(ActivatedRoute);
	private readonly assetService = inject(AssetService);

	assetId = signal<number | null>(null);
	history = signal<EmployeeAsset[]>([]);
	loading = signal(false);
	error = signal('');

	get assetIdValue(): number | null {
		return this.assetId();
	}

	get historyValue(): EmployeeAsset[] {
		return this.history();
	}

	get isLoading(): boolean {
		return this.loading();
	}

	get errorValue(): string {
		return this.error();
	}

	ngOnInit(): void {
		const id = this.route.snapshot.paramMap.get('id');

		if (!id) {
			this.error.set('An asset id is required.');
			return;
		}

		const assetId = Number(id);
		this.assetId.set(assetId);
		this.loading.set(true);
		this.assetService.getAssetHistory(assetId).subscribe({
			next: (history) => {
				this.history.set(history ?? []);
				this.loading.set(false);
			},
			error: () => {
				this.error.set('Unable to load asset history.');
				this.loading.set(false);
			}
		});
	}
}
