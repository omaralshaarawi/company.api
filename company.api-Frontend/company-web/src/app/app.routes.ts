import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'departments', canActivate: [authGuard], loadChildren: () => import('./features/departments/departments.routes').then(m => m.DEPARTMENT_ROUTES) },
    { path: 'employees', canActivate: [authGuard], loadChildren: () => import('./features/employees/employees.routes').then(m => m.EMPLOYEE_ROUTES) },
    { path: '', redirectTo: 'employees', pathMatch: 'full' }
];