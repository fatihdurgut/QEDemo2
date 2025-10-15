import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'authors',
    loadChildren: () => import('./features/authors/authors.module').then(m => m.AuthorsModule),
    canActivate: [authGuard]
  },
  {
    path: 'publishers',
    loadChildren: () => import('./features/publishers/publishers.module').then(m => m.PublishersModule),
    canActivate: [authGuard]
  },
  {
    path: 'titles',
    loadChildren: () => import('./features/titles/titles.module').then(m => m.TitlesModule),
    canActivate: [authGuard]
  },
  {
    path: 'sales',
    loadChildren: () => import('./features/sales/sales.module').then(m => m.SalesModule),
    canActivate: [authGuard]
  },
  {
    path: 'employees',
    loadChildren: () => import('./features/employees/employees.module').then(m => m.EmployeesModule),
    canActivate: [authGuard]
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule),
    canActivate: [authGuard]
  },
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];
