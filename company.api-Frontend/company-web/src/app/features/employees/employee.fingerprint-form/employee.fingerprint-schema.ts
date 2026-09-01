import { required, schema } from '@angular/forms/signals';

export const employeeFingerprintFormSchema = schema<{
  employeeId: number;
  fingerIndex: string;
  deviceId: string;
  enrolledDate: string | null;
  quality: string;
}>((path) => {
  required(path.employeeId, { message: 'Employee is required.' });
  required(path.fingerIndex, { message: 'Finger index is required.' });
});