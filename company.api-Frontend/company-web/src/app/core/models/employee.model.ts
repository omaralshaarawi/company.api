export interface Employee {
employeeId: number;
fullName: string;
position?: string;
email?: string;
phone?: string;
status: string;
departmentName?: string;
}

export interface PagedEmployeesResponse {
  items: Employee[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface CreateEmployeeRequest {
fullName: string;
nationalId?: string;
departmentId?: number | null;
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
nationalId: string;
departmentId: number | null;
position: string;
email: string;
phone: string;
status: string;
}

// Mapping functions between form model and API request models
export function mapFormToCreateRequest(form: EmployeeFormModel): CreateEmployeeRequest {
  return {
    fullName: form.fullName,
    nationalId: form.nationalId,
    departmentId: form.departmentId,
    position: form.position,
    email: form.email,
    phone: form.phone
  };
}

export function mapFormToUpdateRequest(form: EmployeeFormModel): UpdateEmployeeRequest {
  return {
    fullName: form.fullName,
    nationalId: form.nationalId,
    departmentId: form.departmentId,
    position: form.position,
    email: form.email,
    phone: form.phone,
    status: form.status
  };
}