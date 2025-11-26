import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService, type Toast } from '@/app/shared/services/toast.service';
import { SHADCN_UI_DIRECTIVES } from '@/app/shared/ui/shadcn.config';
import { UntilDestroy } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
    selector: 'app-toast-container',
    imports: [CommonModule, SHADCN_UI_DIRECTIVES],
    templateUrl: './toast-container.component.html',
    styleUrls: ['./toast-container.component.scss']
})
export class ToastContainerComponent {
    public readonly toastService = inject(ToastService);

    public readonly toastClassMap: Record<Toast['type'], string> = {
        success: 'text-sm font-medium max-w-md bg-green-500 text-white pointer-events-auto animate-in slide-in-from-right-full duration-300',
        error: 'text-sm font-medium max-w-md bg-red-500 text-white pointer-events-auto animate-in slide-in-from-right-full duration-300',
        warning: 'text-sm font-medium max-w-md bg-yellow-500 text-white pointer-events-auto animate-in slide-in-from-right-full duration-300',
        info: 'text-sm font-medium max-w-md bg-blue-500 text-white pointer-events-auto animate-in slide-in-from-right-full duration-300'
    };

    public trackByToastId(index: number, toast: Toast): string {
        return toast.id;
    }
}
