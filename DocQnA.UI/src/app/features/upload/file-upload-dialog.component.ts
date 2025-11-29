import { Component, inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { FileUploadComponent } from '@/app/shared/components/file-upload/file-upload.component';
import { FileUploadService } from '@/app/shared/components/file-upload/file-upload.service';
import { ToastService } from '@/app/shared/services/toast.service';

@Component({
  selector: 'app-file-upload-dialog',
  standalone: true,
  imports: [CommonModule, FileUploadComponent, MatDialogModule, MatButtonModule],
  templateUrl: './file-upload-dialog.component.html',
})
export class FileUploadDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<FileUploadDialogComponent>);
  private readonly fileUploadService = inject(FileUploadService);
  private readonly toast = inject(ToastService);
  public data = inject(MAT_DIALOG_DATA);

  public file: File | null = null;
  public uploading = false;
  public error: string | null = null;

  public onFileSelected(file: File) {
    this.file = file;
    this.error = null;
  }

  public upload() {
    if (!this.file) {
      return;
    }

    this.uploading = true;
    this.error = null;

    this.fileUploadService.uploadFile(this.file).subscribe({
      next: res => {
        this.toast.success('Upload complete');
        this.uploading = false;
        this.dialogRef.close(res);
      },
      error: () => {
        this.uploading = false;
        this.error = 'Upload failed. Try again.';
      },
    });
  }

  public cancel() {
    this.dialogRef.close(null);
  }
}
