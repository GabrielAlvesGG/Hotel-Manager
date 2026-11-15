import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs';

import { ClientService } from '../../services/client.service';

@Component({
  selector: 'app-client-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './client-register.component.html',
  styleUrl: './client-register.component.scss',
})
export class ClientRegisterComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly clientService = inject(ClientService);

  readonly submitting = signal(false);
  readonly successMessage = signal('');
  readonly errorMessage = signal('');

  readonly form = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(254)]],
    password: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(128)]],
  });

  register(): void {
    if (this.submitting()) {
      return;
    }

    this.successMessage.set('');
    this.errorMessage.set('');

    const values = this.form.getRawValue();

    this.form.patchValue({
      name: values.name.trim(),
      email: values.email.trim().toLowerCase(),
    });

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);

    this.clientService
      .register(this.form.getRawValue())
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe({
        next: (response) => {
          this.successMessage.set(response.message);
          this.form.reset();
        },
        error: (error: HttpErrorResponse) => {
          if (error.status === 409) {
            this.errorMessage.set('Já existe um cliente cadastrado com esse e-mail.');
            return;
          }

          if (error.status === 400) {
            this.errorMessage.set('Confira os dados preenchidos e tente novamente.');
            return;
          }

          this.errorMessage.set('Não foi possível cadastrar o cliente. Tente novamente.');
        },
      });
  }
}
