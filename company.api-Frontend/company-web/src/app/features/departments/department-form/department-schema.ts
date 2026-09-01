import { schema, required, maxLength } from '@angular/forms/signals';
import { CreateDepartmentRequest } from '../../../core/models/departments.model';
export const departmentSchema = schema<CreateDepartmentRequest>((path) => {
    required(path.name, { message: 'Department name is required.' });
    maxLength(path.name, 150);
});
