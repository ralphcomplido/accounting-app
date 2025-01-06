import { Component, ElementRef, inject } from "@angular/core";
import { LayoutService } from "src/app/layout/services/layout.service";
import { AppMenuComponent } from "../app-menu/app-menu.component";

@Component({
  selector: "app-sidebar",
  templateUrl: "./app-sidebar.component.html",
  imports: [AppMenuComponent],
})
export class AppSidebarComponent {
  readonly layoutService = inject(LayoutService);
  readonly el = inject(ElementRef);
}
