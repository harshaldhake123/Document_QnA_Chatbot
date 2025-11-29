import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { FileUploadDialogComponent } from '../upload/file-upload-dialog.component';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-chat-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-page.component.html',
})
export class ChatPageComponent {
  private readonly dialog = inject(MatDialog);

  public documentId: string | null = null;

  public openUploadDialog() {
    this.dialog
      .open(FileUploadDialogComponent, {
        width: '450px',
        disableClose: true,
      })
      .afterClosed()
      .pipe(untilDestroyed(this))
      .subscribe(result => {
        if (result !== null && result !== undefined) {
          this.documentId = result.documentId ?? result.id ?? result;
        }
      });
  }
}
