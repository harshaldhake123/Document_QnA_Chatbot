import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SHADCN_UI_DIRECTIVES } from '@/app/shared/ui/shadcn.config';
import { FileUploadComponent } from '@/app/shared/components/file-upload/file-upload.component';
import { FileUploadService } from '@/app/shared/components/file-upload/file-upload.service';
import { ToastService } from '@/app/shared/services/toast.service';
import { Subject, BehaviorSubject, Observable } from 'rxjs';
import { switchMap, map, catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

interface UploadState {
  readonly file: File | null;
  readonly uploading: boolean;
  readonly success: boolean;
  readonly error: string;
}

const initialUploadState: UploadState = {
  file: null,
  uploading: false,
  success: false,
  error: '',
};

@UntilDestroy()
@Component({
  selector: 'app-upload-page',
  standalone: true,
  imports: [CommonModule, SHADCN_UI_DIRECTIVES, FileUploadComponent],
  templateUrl: './upload-page.component.html',
  styleUrls: ['./upload-page.component.scss'],
})
export class UploadPageComponent implements OnInit {
  private readonly fileSelect$ = new Subject<File>();
  private readonly uploadTrigger$ = new Subject<void>();
  private readonly fileUploadService = inject(FileUploadService);
  private readonly toastService = inject(ToastService);

  public readonly uploadState$: BehaviorSubject<UploadState> = new BehaviorSubject<UploadState>(
    initialUploadState
  );

  public readonly selectedFile$: Observable<File | null> = this.uploadState$.pipe(
    map(state => state.file)
  );

  public readonly uploading$: Observable<boolean> = this.uploadState$.pipe(
    map(state => state.uploading)
  );

  public readonly uploadSuccess$: Observable<boolean> = this.uploadState$.pipe(
    map(state => state.success)
  );

  public readonly uploadError$: Observable<string> = this.uploadState$.pipe(
    map(state => state.error)
  );

  public ngOnInit(): void {
    this.fileSelect$
      .pipe(
        tap(file => {
          this.updateState({ file, uploading: false, success: false, error: '' });
          this.toastService.info(`Selected file: ${file.name}`);
        }),
        untilDestroyed(this)
      )
      .subscribe();

    this.uploadTrigger$
      .pipe(
        switchMap(() => {
          const currentFile = this.uploadState$.value.file;
          if (!currentFile) {
            return of(null);
          }
          this.updateState({ uploading: true, success: false, error: '' });
          this.toastService.info('Uploading file...');
          return this.fileUploadService.uploadFile(currentFile).pipe(
            tap(() => {
              this.updateState({ uploading: false, success: true, file: null });
              this.toastService.success('File uploaded successfully!');
            }),
            catchError(() => {
              this.updateState({ uploading: false, error: 'Upload failed. Please try again.' });
              return of(null);
            })
          );
        }),
        untilDestroyed(this)
      )
      .subscribe();
  }

  private updateState(partial: Partial<UploadState>): void {
    this.uploadState$.next({ ...this.uploadState$.value, ...partial });
  }

  public onFileSelected(file: File): void {
    this.fileSelect$.next(file);
  }

  public upload(): void {
    this.uploadTrigger$.next();
  }
}
