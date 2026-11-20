import { Routes } from '@angular/router';
import { authGuard } from './features/auth/auth.guard';
import { HotelRegisterComponent } from './features/hotels/pages/hotel-register/hotel-register.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'hoteis/cadastro',
    pathMatch: 'full',
  },
  {
    path: 'hoteis/cadastro',
    component: HotelRegisterComponent,
    title: 'Cadastro de hotel',
  },
  {
    path: 'clientes/cadastro',
    loadComponent: () =>
      import('./features/clients/pages/client-register/client-register.component').then(
        (component) => component.ClientRegisterComponent,
      ),
  },
  {
    path: 'clientes/listagem',
    title: 'Listagem de clientes',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/clients/pages/client-list/client-list.component').then(
        (component) => component.ClientListComponent,
      ),
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/login/login.component').then(
        (component) => component.LoginComponent,
      ),
  },
];
