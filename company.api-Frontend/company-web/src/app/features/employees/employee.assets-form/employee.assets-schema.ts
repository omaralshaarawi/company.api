import { required, schema } from '@angular/forms/signals';

export const employeeAssetFormSchema = schema<{ assetId: string; notes: string }>((path) => {
  required(path.assetId, { message: 'Asset is required.' });
});