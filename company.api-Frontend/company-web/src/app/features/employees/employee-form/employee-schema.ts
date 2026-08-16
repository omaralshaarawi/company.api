import { schema, required, maxLength, email } from '@angular/forms/signals';
import { EmployeeFormModel } from '../../../core/models/employee.model';
export const employeeSchema = schema<EmployeeFormModel>((path) => {
    required(path.fullName, { message: 'Full name is required.' });
    maxLength(path.fullName, 150);
    email(path.email, { message: 'Enter a valid email address.' });
});