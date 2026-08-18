import { schema, required, maxLength } from '@angular/forms/signals';
import { department } from '../../../core/models/departments.model';
export const departmentSchema = schema<department>((path) => {
    required(path.name, { message: 'Department name is required.' });
    maxLength(path.name, 150);
});
