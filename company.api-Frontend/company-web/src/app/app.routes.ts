import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'departments', canActivate: [authGuard], loadChildren: () => import('./features/departments/departments.routes').then(m => m.DEPARTMENT_ROUTES) },
    { path: 'employees', canActivate: [authGuard], loadChildren: () => import('./features/employees/employees.routes').then(m => m.EMPLOYEE_ROUTES) },
    { path: 'asset-types', canActivate: [authGuard], loadChildren: () => import('./features/assetTypes/asset-types.routes').then(m => m.ASSET_TYPE_ROUTES) },
    { path: 'assets', canActivate: [authGuard], loadChildren: () => import('./features/assets/assets.routes').then(m => m.ASSETS_ROUTES) },
    { path: 'reports', canActivate: [authGuard], loadChildren: () => import('./features/reports/reports.routes').then(m => m.REPORTS_ROUTES) },
    { path: '', redirectTo: 'employees', pathMatch: 'full' }
];