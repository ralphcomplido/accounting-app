import { DestroyRef, Directive, ElementRef, inject, Input, Renderer2, SimpleChanges } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { IdentityService } from "@identity";
import { Subscription } from "rxjs";

@Directive({
  selector: "[hideByRoles]",
  standalone: true,
})
export class HideByRolesDirective {
  #identityService = inject(IdentityService);
  #el = inject(ElementRef);
  #destroyRef = inject(DestroyRef);
  #renderer = inject(Renderer2);
  #subscription?: Subscription;
  #originalDisplay = this.#el.nativeElement.style.display;

  @Input({ required: true }) roles: Array<string> | string;

  ngOnChanges(changes: SimpleChanges) {
    if (changes["roles"]) {
      if (this.#subscription) this.#subscription.unsubscribe();

      const roles = Array.isArray(this.roles) ? this.roles : [this.roles];

      this.#subscription = this.#identityService
        .watchLoggedInToAnyRole$(roles)
        .pipe(takeUntilDestroyed(this.#destroyRef))
        .subscribe({
          next: isInRole => {
            if (isInRole) {
              this.#renderer.setStyle(this.#el.nativeElement, "display", "none");
            } else if (this.#originalDisplay?.length) {
              this.#renderer.setStyle(this.#el.nativeElement, "display", this.#originalDisplay);
            } else {
              this.#renderer.removeStyle(this.#el.nativeElement, "display");
            }
          },
        });
    }
  }
}
