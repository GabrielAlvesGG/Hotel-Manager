import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ClientRegisterRequest, ClientRegisterResponse } from '../models/client-register.model';

@Injectable({
  providedIn: 'root',
})
export class HotelService {
  private readonly http = inject(HttpClient);

  private readonly registerUrl = 'https://localhost:7005/api/client-register';

  register(data: ClientRegisterRequest): Observable<ClientRegisterResponse> {
    return this.http.post<ClientRegisterResponse>(this.registerUrl, data);
  }
}
