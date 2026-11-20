import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { EMPTY, Observable, Subject, catchError, distinctUntilChanged, map, of, switchMap, timer } from 'rxjs';

import { AuthService } from '../../../auth/services/auth.service';
import { ClientListResponse } from '../../models/client.model';
import { ClientService } from '../../services/client.service';

@Component({
  selector: 'app-client-list',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './client-list.component.html',
  styleUrl: './client-list.component.scss',
})
export class ClientListComponent {
  private readonly clients = inject(ClientService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly requests = new Subject<number>();
  private readonly pageSize = 20;

  readonly searchControl = new FormControl('', { nonNullable: true });
  readonly search = signal('');
  readonly page = signal(1);
  readonly result = signal<ClientListResponse | null>(null);
  readonly status = signal<'loading' | 'ready' | 'error'>('loading');
  readonly totalPages = computed(() => this.lastPage(this.result()?.totalCount ?? 0));

  constructor() {
    this.requests.pipe(
      switchMap((delay) => {
        this.status.set('loading');
        this.result.set(null);
        const search = this.search();
        const page = this.page();

        return (delay ? timer(delay) : of(0)).pipe(
          switchMap(() => this.loadPage(search, page)),
          catchError((error: HttpErrorResponse) => {
            if (error.status === 401) {
              this.auth.clearSession();
              void this.router.navigateByUrl('/login');
            } else {
              this.status.set('error');
            }

            return EMPTY;
          }),
        );
      }),
      takeUntilDestroyed(),
    ).subscribe((result) => {
      this.result.set(result);
      this.status.set('ready');
    });

    this.searchControl.valueChanges.pipe(
      map((value) => value.trim()),
      distinctUntilChanged(),
      takeUntilDestroyed(),
    ).subscribe((search) => {
      this.search.set(search);
      this.page.set(1);
      this.requests.next(250);
    });

    this.requests.next(0);
  }

  changePage(page: number): void {
    this.page.set(page);
    this.retry();
  }

  retry(): void {
    this.requests.next(0);
  }

  private loadPage(search: string, page: number): Observable<ClientListResponse> {
    return this.clients.list(search, page, this.pageSize).pipe(
      switchMap((result) => {
        const lastPage = this.lastPage(result.totalCount);
        if (page <= lastPage) {
          return of(result);
        }

        this.page.set(lastPage);
        return this.loadPage(search, lastPage);
      }),
    );
  }

  private lastPage(totalCount: number): number {
    return Math.max(1, Math.ceil(totalCount / this.pageSize));
  }
}
