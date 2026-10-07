import { schema, required, maxLength, email } from '@angular/forms/signals';
import { EmployeeFormModel } from '../../../core/models/employee.model';
export const employeeSchema = schema<EmployeeFormModel>((path) => {
    required(path.fullName, { message: $localize`Full name is required.` });
    maxLength(path.fullName, 150);
    required(path.nationalId, { message: $localize`National ID is required.` });
    email(path.email, { message: $localize`Enter a valid email address.` });
});