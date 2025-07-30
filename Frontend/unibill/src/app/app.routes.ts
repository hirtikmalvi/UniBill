import { Routes } from '@angular/router';
import { RegisterComponent } from './Components/Auth/register/register.component';
import { LoginComponent } from './Components/Auth/login/login.component';
import { RegisterBusinessComponent } from './Components/Business/register-business/register-business.component';
import { MyBusinessComponent } from './Components/Business/my-business/my-business.component';
import { DashboardComponent } from './Components/Business/dashboard/dashboard.component';
import { ErrorComponent } from './Components/error/error.component';
import { AuthComponent } from './Components/Auth/auth/auth.component';
import { validateUserGuard } from './Guards/User/validate-user.guard';
import { hasBusinessGuard } from './Guards/Business/has-business.guard';

export const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [validateUserGuard, hasBusinessGuard],
  },
  {
    path: 'auth',
    component: AuthComponent,
    children: [
      {
        path: 'register',
        component: RegisterComponent,
      },
      {
        path: 'login',
        component: LoginComponent,
      },
      {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full',
      },
    ],
  },
  {
    path: 'business',
    children: [
      {
        path: 'register-business',
        component: RegisterBusinessComponent,
        canActivate: [validateUserGuard],
      },
      {
        path: 'my-business',
        component: MyBusinessComponent,
        canActivate: [validateUserGuard, hasBusinessGuard],
      },
      {
        path: '',
        redirectTo: 'my-business',
        pathMatch: 'full',
      },
    ],
  },
  {
    path: 'error-page',
    component: ErrorComponent,
  },
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: '/error-page',
    pathMatch: 'full',
  },
];
