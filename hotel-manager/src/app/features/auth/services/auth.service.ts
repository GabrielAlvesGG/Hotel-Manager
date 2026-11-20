import { inject, Injectable, signal } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
} from '@angular/common/http';
import {
  Observable,
  catchError,
  finalize,
  shareReplay,
  switchMap,
  tap,
  throwError,
} from 'rxjs';

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

  private readonly clientState =
    signal<AuthenticatedClient | null>(null);

  private sessionRequest?: Observable<AuthenticatedClient>;

  readonly currentClient = this.clientState.asReadonly();

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.getCsrfToken().pipe(
      switchMap((csrf) =>
        this.http.post<LoginResponse>(
          `${this.apiUrl}/login`,
          request,
          {
            withCredentials: true,
            headers: {
              'X-CSRF-TOKEN': csrf.token,
            },
          },
        ),
      ),
      tap((response) => {
        this.clientState.set({
          id: String(response.id),
          name: response.name,
          email: response.email,
        });
      }),
    );
  }

  getCurrentClient(): Observable<AuthenticatedClient> {
    return (this.sessionRequest ??= this.http
      .get<AuthenticatedClient>(
        `${this.apiUrl}/me`,
        {
          withCredentials: true,
        },
      )
      .pipe(
        tap((client) => this.clientState.set(client)),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            this.clientState.set(null);
          }

          return throwError(() => error);
        }),
        finalize(() => { this.sessionRequest = undefined; }),
        shareReplay({ bufferSize: 1, refCount: true }),
      ));
  }

  clearSession(): void {
    this.clientState.set(null);
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
      tap(() => this.clientState.set(null)),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.clientState.set(null);
        }

        return throwError(() => error);
      }),
    );
  }

  private getCsrfToken(): Observable<CsrfResponse> {
    return this.http.get<CsrfResponse>(
      `${this.apiUrl}/csrf`,
      {
        withCredentials: true,
      },
    );
  }
}