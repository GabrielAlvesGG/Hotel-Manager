import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ClientListResponse, CreateClientRequest, CreateClientResponse } from '../models/client.model';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7005/api/clients';

  list(search: string, page: number, pageSize: number): Observable<ClientListResponse> {
    return this.http.get<ClientListResponse>(this.apiUrl, {
      params: { search, page, pageSize },
      withCredentials: true,
    });
  }

  register(request: CreateClientRequest): Observable<CreateClientResponse> {
    return this.http.post<CreateClientResponse>(this.apiUrl, request);
  }
}
