import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { LayoutService } from "src/app/layout/services/layout.service";
import { MenuService } from "src/app/layout/services/menu.service";
import { AppMenuItemComponent } from "../app-menu-item/app-menu-item.component";

@Component({
  selector: "app-menu",
  standalone: true,
  templateUrl: "./app-menu.component.html",
  imports: [CommonModule, AppMenuItemComponent],
})
export class AppMenuComponent {
  readonly layoutService = inject(LayoutService);
  readonly #menuService = inject(MenuService);

  readonly menuItems$ = this.#menuService.watchMenuItems$();
}
