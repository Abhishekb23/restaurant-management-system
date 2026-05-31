import { Routes } from '@angular/router';
import { Dashboard} from './dashboard/dashboard';
import { AuthTabs } from './auth/auth-tabs/auth-tabs';
import { UserDashboard } from './dashboards/user-dashboard/user-dashboard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: AuthTabs
  },

  {
    path: 'dashboard',
    component: UserDashboard
  }
];