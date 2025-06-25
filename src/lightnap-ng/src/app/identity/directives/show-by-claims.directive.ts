import { DestroyRef, Directive, ElementRef, inject, Input, Renderer2, SimpleChanges } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { IdentityService } from "@identity";
import { Subscription } from "rxjs";

@Directive({
  selector: "[showByClaims]",
  standalone: true,
})
export class ShowByClaimsDirective {
  #identityService = inject(IdentityService);
  #el = inject(ElementRef);
  #destroyRef = inject(DestroyRef);
  #renderer = inject(Renderer2);
  #subscription?: Subscription;
  #originalDisplay = this.#el.nativeElement.style.display;

  @Input({ required: true }) claims?: Array<[string, string]> | [string, string];

  ngOnChanges(changes: SimpleChanges) {
    if (changes["claims"]) {
      if (this.#subscription) this.#subscription.unsubscribe();

      if (!this.claims || this.claims.length === 0) {
        console.warn("ShowByClaimsDirective: No claims provided or empty array. Element will not be shown.");
        return;
      }

      const claims = Array.isArray(this.claims[0]) ? (this.claims as Array<[string, string]>) : ([this.claims] as Array<[string, string]>);

      this.#subscription = this.#identityService
        .watchLoggedInWithAnyClaim$(claims)
        .pipe(takeUntilDestroyed(this.#destroyRef))
        .subscribe({
          next: isInClaim => {
            if (isInClaim) {
              if (this.#originalDisplay?.length) {
                this.#renderer.setStyle(this.#el.nativeElement, "display", this.#originalDisplay);
              } else {
                this.#renderer.removeStyle(this.#el.nativeElement, "display");
              }
            } else {
              this.#renderer.setStyle(this.#el.nativeElement, "display", "none");
            }
          },
        });
    }
  }
}
