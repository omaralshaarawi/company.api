import { schema, required, maxLength } from '@angular/forms/signals';
import { asset } from '../../../core/models/assets.model';

export const assetSchema = schema<asset>((path: any) => {
    required(path.assetName, { message: $localize`Asset name is required.` });
    maxLength(path.assetName, 150);
    required(path.assetTypeId, {message: $localize`Asset type ID is required.`});
});
