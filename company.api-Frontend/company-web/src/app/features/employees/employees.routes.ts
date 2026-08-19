// features/employees/employees.routes.ts
import { Routes } from '@angular/router';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { EmployeeAssetsComponent } from './employee.assets/employee.assets.component';
import { EmployeeAssetsFormComponent } from './employee.assets-form/employee.assets-form.component';
export const EMPLOYEE_ROUTES: Routes = [
    { path: '', component: EmployeeListComponent },
    { path: 'new', component: EmployeeFormComponent },
    { path: ':id/edit', component: EmployeeFormComponent },
    { path: ':id/assets', component: EmployeeAssetsComponent },
    { path: ':id/assets/new', component: EmployeeAssetsFormComponent }
];