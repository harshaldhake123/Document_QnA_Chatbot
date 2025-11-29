import { Component, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, BehaviorSubject, Observable } from 'rxjs';
import { UntilDestroy } from '@ngneat/until-destroy';

const MAX_FILE_SIZE = 5 * 1024 * 1024;
const ALLOWED_FILE_TYPE = 'application/pdf';

@UntilDestroy()
@Component({
  selector: 'app-file-upload',
  imports: [CommonModule],
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss'],
})
export class FileUploadComponent {
  @Output() public readonly fileSelected = new Subject<File>();

  private readonly errorSubject$ = new BehaviorSubject<string>('');
  public readonly errorMessage$: Observable<string> = this.errorSubject$.asObservable();

  public onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    this.errorSubject$.next('');

    if (file.type !== ALLOWED_FILE_TYPE) {
      this.errorSubject$.next('Only PDF files are supported.');
      return;
    }

    if (file.size > MAX_FILE_SIZE) {
      this.errorSubject$.next('File size must be 5MB or less.');
      return;
    }

    this.fileSelected.next(file);
  }
}
