import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'upload',
    loadComponent: () =>
      import('./features/upload/file-upload-dialog.component').then(
        m => m.FileUploadDialogComponent
      ),
  },
  {
    path: 'chat',
    loadComponent: () =>
      import('./features/chat/chat-page.component').then(m => m.ChatPageComponent),
  },
  {
    path: '',
    redirectTo: 'upload',
    pathMatch: 'full',
  },
];
