import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, switchMap } from 'rxjs';

import {
  AuthenticatedClient,
  CsrfResponse,
  LoginRequest,
  LoginResponse,
  LogoutResponse,
} from '../models/auth.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7005/api/auth';

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.getCsrfToken().pipe(
      switchMap((csrf) =>
        this.http.post<LoginResponse>(`${this.apiUrl}/login`, request, {
          withCredentials: true,
          headers: {
            'X-CSRF-TOKEN': csrf.token,
          },
        }),
      ),
    );
  }

  getCurrentClient(): Observable<AuthenticatedClient> {
    return this.http.get<AuthenticatedClient>(`${this.apiUrl}/me`, {
      withCredentials: true,
    });
  }

  logout(): Observable<LogoutResponse> {
    return this.getCsrfToken().pipe(
      switchMap((csrf) =>
        this.http.post<LogoutResponse>(
          `${this.apiUrl}/logout`,
          {},
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

  private getCsrfToken(): Observable<CsrfResponse> {
    return this.http.get<CsrfResponse>(`${this.apiUrl}/csrf`, {
      withCredentials: true,
    });
  }
}
