import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'upload',
        loadComponent: () => import('./features/upload/upload-page.component').then((m) => m.UploadPageComponent)
    },
    {
        path: 'chat',
        loadComponent: () => import('./features/chat/chat-page.component').then((m) => m.ChatPageComponent)
    },
    {
        path: '',
        redirectTo: 'upload',
        pathMatch: 'full'
    }
];
