export interface ClientRegisterRequest {
  cnpj: string;
  name: string;
  localization: string;
  hotelType: string;
}

export interface ClientRegisterResponse {
  message: string;
}
