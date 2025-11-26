import { Injectable, inject } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastService } from '../services/toast.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    private readonly toastService = inject(ToastService);

    public intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {
                const message = this.getMessage(error);
                this.toastService.error(message);
                return throwError(() => error);
            })
        );
    }

    private getMessage(error: HttpErrorResponse): string {
        switch (error.status) {
            case 0:
                return 'Network error. Please check your connection.';
            case HttpStatusCode.BadRequest:
                return (error.error as { message?: string; })?.message ?? 'Invalid request.';
            case HttpStatusCode.Unauthorized:
                return 'Unauthorized. Please log in again.';
            case HttpStatusCode.Forbidden:
                return 'Access denied.';
            case HttpStatusCode.NotFound:
                return 'Resource not found.';
            case HttpStatusCode.InternalServerError:
                return 'Server error. Please try again later.';
            default:
                return `Unexpected error: ${error.message}`;
        }
    }
}
