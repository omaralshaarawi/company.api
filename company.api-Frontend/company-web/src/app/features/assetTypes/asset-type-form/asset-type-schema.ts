import { schema, required, maxLength } from '@angular/forms/signals';
import { assetType } from '../../../core/models/assetTypes.model';
export const assetTypeSchema = schema<assetType>((path) => {
    required(path.typeName, { message: 'Asset type name is required.' });
    maxLength(path.typeName, 150);
});
