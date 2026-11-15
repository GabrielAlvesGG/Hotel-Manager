import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';

import { HotelService } from '../../services/hotel.service';

@Component({
  selector: 'app-hotel-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './hotel-register.component.html',
  styleUrl: './hotel-register.component.scss',
})
export class HotelRegisterComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly hotelService = inject(HotelService);

  readonly sending = signal(false);
  readonly successMessage = signal('');
  readonly errorMessage = signal('');

  readonly form = this.formBuilder.nonNullable.group({
    cnpj: ['', Validators.required],
    name: ['', [Validators.required, Validators.minLength(3)]],
    localization: ['', Validators.required],
    hotelType: ['', Validators.required],
  });

  register(): void {
    if (this.sending()) {
      return;
    }

    this.successMessage.set('');
    this.errorMessage.set('');

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const values = this.form.getRawValue();

    const request = {
      cnpj: values.cnpj.trim(),
      name: values.name.trim(),
      localization: values.localization.trim(),
      hotelType: values.hotelType,
    };

    if (!request.cnpj || request.name.length < 3 || !request.localization) {
      this.errorMessage.set('Preencha os campos com valores válidos, sem usar somente espaços.');
      return;
    }

    this.sending.set(true);

    this.hotelService
      .register(request)
      .pipe(finalize(() => this.sending.set(false)))
      .subscribe({
        next: (response) => {
          this.successMessage.set(response.message);
          this.form.reset();
        },
        error: () => {
          this.errorMessage.set(
            'Não foi possível enviar o cadastro. Verifique a conexão com a API e tente novamente.',
          );
        },
      });
  }

  clear(): void {
    if (this.sending()) {
      return;
    }

    this.form.reset();
    this.successMessage.set('');
    this.errorMessage.set('');
  }
}
