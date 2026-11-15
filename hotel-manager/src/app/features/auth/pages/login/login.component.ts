import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { finalize, switchMap } from 'rxjs';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);

  readonly submitting = signal(false);
  readonly successMessage = signal('');
  readonly errorMessage = signal('');

  readonly form = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email, Validators.maxLength(254)]],
    password: ['', [Validators.required, Validators.maxLength(128)]],
  });

  login(): void {
    if (this.submitting()) {
      return;
    }

    this.successMessage.set('');
    this.errorMessage.set('');

    this.form.controls.email.setValue(this.form.controls.email.value.trim().toLowerCase());

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);

    let credentialsAccepted = false;

    this.authService
      .login(this.form.getRawValue())
      .pipe(
        switchMap(() => {
          credentialsAccepted = true;

          // A senha não é mais necessária no formulário.
          this.form.controls.password.reset();

          return this.authService.getCurrentClient();
        }),
        finalize(() => this.submitting.set(false)),
      )
      .subscribe({
        next: (client) => {
          this.successMessage.set(`Olá, ${client.name}! Sua autenticação foi confirmada.`);
        },
        error: (error: HttpErrorResponse) => {
          if (credentialsAccepted) {
            this.errorMessage.set(
              'O login foi aceito, mas não foi possível confirmar a sessão. Confira se os cookies estão habilitados.',
            );
            return;
          }

          if (error.status === 401) {
            this.errorMessage.set('E-mail ou senha inválidos.');
            return;
          }

          if (error.status === 400) {
            this.errorMessage.set(
              'Não foi possível validar a solicitação. Atualize a página e tente novamente.',
            );
            return;
          }

          if (error.status === 0) {
            this.errorMessage.set(
              'Não foi possível acessar a API. Confira se ela está em execução e se o navegador aceitou seu certificado HTTPS.',
            );
            return;
          }

          this.errorMessage.set('Não foi possível entrar. Tente novamente.');
        },
      });
  }
}
