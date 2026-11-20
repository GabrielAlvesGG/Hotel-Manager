export interface CreateClientRequest {
  name: string;
  email: string;
  password: string;
}

export interface CreateClientResponse {
  id: number;
  message: string;
}

export interface ClientListItem {
  name: string;
  email: string;
}

export interface ClientListResponse {
  items: ClientListItem[];
  totalCount: number;
  page: number;
  pageSize: number;
}
