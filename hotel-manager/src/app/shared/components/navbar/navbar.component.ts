import {
  Component,
  OnInit,
  inject,
  signal,
} from '@angular/core';
import {
  Router,
  RouterLink,
  RouterLinkActive,
} from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs';

import { AuthService } from '../../../features/auth/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly currentClient = this.authService.currentClient;

  readonly checkingSession = signal(true);
  readonly loggingOut = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.authService
      .getCurrentClient()
      .pipe(finalize(() => this.checkingSession.set(false)))
      .subscribe({
        error: (error: HttpErrorResponse) => {
          if (error.status !== 401) {
            this.errorMessage.set(
              'Não foi possível verificar a sessão. Atualize a página para tentar novamente.',
            );
          }
        },
      });
  }

  logout(): void {
    if (this.loggingOut()) {
      return;
    }

    this.errorMessage.set('');
    this.loggingOut.set(true);

    this.authService
      .logout()
      .pipe(finalize(() => this.loggingOut.set(false)))
      .subscribe({
        next: () => {
          void this.router.navigateByUrl('/login');
        },
        error: (error: HttpErrorResponse) => {
          if (error.status === 401) {
            void this.router.navigateByUrl('/login');
            return;
          }

          this.errorMessage.set(
            'Não foi possível sair. Tente novamente.',
          );
        },
      });
  }
}