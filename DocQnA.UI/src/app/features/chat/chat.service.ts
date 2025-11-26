import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@/environments/environment';
import type { ChatQueryResponse } from '@/app/shared/api/types';

@Injectable({ providedIn: 'root' })
export class ChatService {
    private readonly http = inject(HttpClient);
    private readonly apiBaseUrl = environment.apiBaseUrl;

    public queryDocument(documentId: string, question: string): Observable<ChatQueryResponse> {
        return this.http.post<ChatQueryResponse>(
            `${this.apiBaseUrl}/api/document/${documentId}/query`,
            { question }
        );
    }
}
