import { Routes } from '@angular/router';
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
];
