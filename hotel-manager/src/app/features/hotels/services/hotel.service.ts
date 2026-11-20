import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, switchMap } from 'rxjs';

import {
  ClientRegisterRequest,
  ClientRegisterResponse,
} from '../models/client-register.model';

interface CsrfResponse {
  token: string;
}

@Injectable({
  providedIn: 'root',
})
export class HotelService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7005';

  register(
    request: ClientRegisterRequest,
  ): Observable<ClientRegisterResponse> {
    return this.http
      .get<CsrfResponse>(
        `${this.apiUrl}/api/auth/csrf`,
        {
          withCredentials: true,
        },
      )
      .pipe(
        switchMap((csrf) =>
          this.http.post<ClientRegisterResponse>(
            `${this.apiUrl}/api/client-register`,
            request,
            {
              withCredentials: true,
              headers: {
                'X-CSRF-TOKEN': csrf.token,
              },
            },
          ),
        ),
      );
  }
}