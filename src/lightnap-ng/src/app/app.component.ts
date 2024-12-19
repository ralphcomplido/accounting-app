import { Component, inject, OnInit } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { RouterOutlet } from "@angular/router";
import { BlockUiService } from "@core";
import { PrimeNGConfig } from "primeng/api";
import { BlockUIModule } from "primeng/blockui";
import { ToastModule } from "primeng/toast";

@Component({
  standalone: true,
  selector: "app-root",
  templateUrl: "./app.component.html",
  imports: [RouterOutlet, BlockUIModule, ToastModule]
})
export class AppComponent implements OnInit {
  #primengConfig = inject(PrimeNGConfig);
  #blockUiService = inject(BlockUiService);

  showBlockUi = false;
  blockUiIconClass = "";
  blockUiMessage = "";

  constructor() {
    this.#blockUiService.onShow$.pipe(takeUntilDestroyed()).subscribe(blockUiParams => {
      this.showBlockUi = true;
      this.blockUiMessage = blockUiParams.message ?? "Processing...";
      this.blockUiIconClass = blockUiParams.iconClass ?? "pi pi-spin pi-spinner text-4xl";
    });

    this.#blockUiService.onHide$.subscribe(() => {
      this.showBlockUi = false;
    });
  }

  ngOnInit() {
    this.#primengConfig.ripple = true;
  }
}
