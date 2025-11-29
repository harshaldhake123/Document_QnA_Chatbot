import { computed, Directive, input } from '@angular/core';

import { cn } from '@/lib/utils';

@Directive({
  standalone: true,
  selector: 'div[appUbCard]',
  host: {
    '[class]': 'computedClass()',
  },
})
export class UbCardDirective {
  public readonly class = input<string>();
  protected readonly computedClass = computed(() =>
    cn('bg-card text-card-foreground rounded-xl border shadow-sm', this.class())
  );
}

@Directive({
  standalone: true,
  selector: 'div[appUbCardHeader]',
  host: {
    '[class]': 'computedClass()',
  },
})
export class UbCardHeaderDirective {
  public readonly class = input<string>();
  protected readonly computedClass = computed(() => cn('flex flex-col gap-1.5 p-6', this.class()));
}

@Directive({
  standalone: true,
  selector: 'h3[appUbCardTitle]',
  host: {
    '[class]': 'computedClass()',
  },
})
export class UbCardTitleDirective {
  public readonly class = input<string>();
  protected readonly computedClass = computed(() => cn('leading-none font-semibold', this.class()));
}

@Directive({
  standalone: true,
  selector: 'p[appUbCardDescription]',
  host: {
    '[class]': 'computedClass()',
  },
})
export class UbCardDescriptionDirective {
  public readonly class = input<string>();
  protected readonly computedClass = computed(() =>
    cn('text-muted-foreground text-sm', this.class())
  );
}

@Directive({
  standalone: true,
  selector: 'div[appUbCardContent]',
  host: {
    '[class]': 'computedClass()',
  },
})
export class UbCardContentDirective {
  public readonly class = input<string>();
  protected readonly computedClass = computed(() => cn('p-6 pt-0', this.class()));
}

@Directive({
  standalone: true,
  selector: 'div[appUbCardFooter]',
  host: {
    '[class]': 'computedClass()',
  },
})
export class UbCardFooterDirective {
  public readonly class = input<string>();
  protected readonly computedClass = computed(() => cn('flex items-center p-6 pt-0', this.class()));
}
