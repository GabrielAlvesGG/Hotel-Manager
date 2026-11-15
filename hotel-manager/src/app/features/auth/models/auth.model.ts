export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  id: number;
  name: string;
  email: string;
  message: string;
}

export interface AuthenticatedClient {
  id: string;
  name: string;
  email: string;
}

export interface CsrfResponse {
  token: string;
}

export interface LogoutResponse {
  message: string;
}
