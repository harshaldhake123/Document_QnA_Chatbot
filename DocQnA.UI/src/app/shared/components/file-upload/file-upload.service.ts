import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@/environments/environment';
import type { FileUploadResponse } from '@/app/shared/api/types';

@Injectable({ providedIn: 'root' })
export class FileUploadService {
    private readonly http = inject(HttpClient);
    private readonly apiBaseUrl = environment.apiBaseUrl;

    public uploadFile(file: File): Observable<FileUploadResponse> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.post<FileUploadResponse>(
            `${this.apiBaseUrl}/api/document/upload`,
            formData
        );
    }
}
