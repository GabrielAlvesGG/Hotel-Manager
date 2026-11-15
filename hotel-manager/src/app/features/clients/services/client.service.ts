import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { CreateClientRequest, CreateClientResponse } from '../models/client.model';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7005/api/clients';

  register(request: CreateClientRequest): Observable<CreateClientResponse> {
    return this.http.post<CreateClientResponse>(this.apiUrl, request);
  }
}
