import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface Toast {
  readonly id: string;
  readonly message: string;
  readonly type: 'success' | 'error' | 'warning' | 'info';
  readonly duration?: number;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private readonly toasts$ = new BehaviorSubject<Toast[]>([]);
  public readonly toasts: Observable<Toast[]> = this.toasts$.asObservable();

  public success(message: string, duration = 5000): void {
    this.show(message, 'success', duration);
  }

  public error(message: string, duration = 5000): void {
    this.show(message, 'error', duration);
  }

  public warning(message: string, duration = 5000): void {
    this.show(message, 'warning', duration);
  }

  public info(message: string, duration = 5000): void {
    this.show(message, 'info', duration);
  }

  public remove(id: string): void {
    const current = this.toasts$.value;
    this.toasts$.next(current.filter(toast => toast.id !== id));
  }

  private show(message: string, type: Toast['type'], duration: number): void {
    const id = `${Date.now()}-${Math.random()}`;
    const toast: Toast = { id, message, type, duration };

    const current = this.toasts$.value;
    this.toasts$.next([...current, toast]);

    if (duration > 0) {
      setTimeout(() => this.remove(id), duration);
    }
  }
}
