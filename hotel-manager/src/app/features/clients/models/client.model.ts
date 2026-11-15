export interface CreateClientRequest {
  name: string;
  email: string;
  password: string;
}

export interface CreateClientResponse {
  id: number;
  message: string;
}
