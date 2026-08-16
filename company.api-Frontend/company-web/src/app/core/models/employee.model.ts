export interface Employee {
employeeId: number;
fullName: string;
position?: string;
email?: string;
phone?: string;
status: string;
departmentName?: string;
}
export interface CreateEmployeeRequest {
fullName: string;
nationalId?: string;
departmentId?: number;
position?: string;
email?: string;
phone?: string;
hireDate?: string; // ISO date string
}
export interface UpdateEmployeeRequest extends CreateEmployeeRequest {
status: string;
}
// Shape used specifically by the Signal Forms form() call — every field
// has a concrete value, never undefined (see the note below the form component).
export interface EmployeeFormModel {
fullName: string;
departmentId: number | null;
position: string;
email: string;
phone: string;
status: string;
}