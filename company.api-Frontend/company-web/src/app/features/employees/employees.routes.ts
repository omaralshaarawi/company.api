// features/employees/employees.routes.ts
import { Routes } from '@angular/router';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { EmployeeAssetsComponent } from './employee.assets/employee.assets.component';
import { EmployeeAssetsFormComponent } from './employee.assets-form/employee.assets-form.component';
import { EmployeeFingerprintFormComponent } from './employee.fingerprint-form/employee.fingerprint-form.component';
import { AttendancelogListComponent } from './employee.attendanceLog/attendancelog-list.component';
import { EmployeeFormReactiveComponent } from './employee-form-reactive/employee-form-reactive.component';
export const EMPLOYEE_ROUTES: Routes = [
    { path: '', component: EmployeeListComponent },
    { path: 'new', component: EmployeeFormReactiveComponent },
    { path: ':id/edit', component: EmployeeFormReactiveComponent },
    { path: ':id/assets', component: EmployeeAssetsComponent },
    { path: ':id/assets/new', component: EmployeeAssetsFormComponent },
    { path: ':id/fingerprint/new', component: EmployeeFingerprintFormComponent },
    { path: ':id/attendance', component: AttendancelogListComponent }
];