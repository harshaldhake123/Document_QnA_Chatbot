import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { SHADCN_UI_DIRECTIVES } from '@/app/shared/ui/shadcn.config';
import { ChatService } from './chat.service';
import { Subject, BehaviorSubject, Observable } from 'rxjs';
import { map, finalize, filter, switchMap, tap } from 'rxjs/operators';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

interface Message {
  readonly text: string;
  readonly sender: 'user' | 'assistant';
}
@UntilDestroy()
@Component({
  selector: 'app-chat-page',
  imports: [CommonModule, ReactiveFormsModule, SHADCN_UI_DIRECTIVES],
  templateUrl: './chat-page.component.html',
  styleUrls: ['./chat-page.component.scss'],
})
export class ChatPageComponent implements OnInit {
  private readonly messageSend$ = new Subject<string>();
  private readonly chatService = inject(ChatService);
  private readonly loadingSubject$ = new BehaviorSubject<boolean>(false);
  private readonly messagesSubject$ = new BehaviorSubject<Message[]>([]);
  private readonly inputValueSubject$ = new BehaviorSubject<string>('');

  public readonly messages$: Observable<Message[]> = this.messagesSubject$.asObservable();
  public readonly loading$: Observable<boolean> = this.loadingSubject$.asObservable();
  public readonly isInputEmpty$: Observable<boolean> = this.inputValueSubject$
    .asObservable()
    .pipe(map(value => !value.trim()));
  public readonly input = new FormControl<string>('', {
    validators: [Validators.minLength(1)],
    nonNullable: true,
  });
  public readonly documentId = new FormControl<string>('', {
    validators: [Validators.required],
    nonNullable: true,
  });

  public ngOnInit(): void {
    this.input.valueChanges
      .pipe(
        tap(value => this.inputValueSubject$.next(value)),
        untilDestroyed(this)
      )
      .subscribe();

    this.messageSend$
      .pipe(
        filter(question => !!question && this.documentId.valid),
        tap(question => {
          this.messagesSubject$.next([
            ...this.messagesSubject$.value,
            { text: question, sender: 'user' },
          ]);
          this.loadingSubject$.next(true);
        }),
        switchMap(question => {
          const docId = this.documentId.value;
          return this.chatService.queryDocument(docId, question).pipe(
            map(res => res.reply),
            tap(reply => {
              this.messagesSubject$.next([
                ...this.messagesSubject$.value,
                { text: reply, sender: 'assistant' },
              ]);
            }),
            finalize(() => this.loadingSubject$.next(false))
          );
        }),
        untilDestroyed(this)
      )
      .subscribe({
        error: () => {
          this.loadingSubject$.next(false);
        },
      });
  }

  public sendMessage(): void {
    const text = this.input.value.trim();
    if (!text || !this.documentId.valid) {
      return;
    }
    this.messageSend$.next(text);
    this.input.reset();
    this.inputValueSubject$.next('');
  }

  public trackByIndex(index: number, _: Message): number {
    return index;
  }
}
