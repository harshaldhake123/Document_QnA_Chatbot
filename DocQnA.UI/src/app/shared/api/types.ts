export interface ApiResponse<T> {
  readonly data: T;
  readonly message?: string;
}

export interface ChatQueryRequest {
  readonly question: string;
}

export interface ChatQueryResponse {
  readonly reply: string;
}

export interface FileUploadResponse {
  readonly documentId: string;
  readonly fileName: string;
}
