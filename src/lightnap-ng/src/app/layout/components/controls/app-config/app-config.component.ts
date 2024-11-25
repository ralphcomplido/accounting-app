import { CommonModule } from "@angular/common";
import { Component, inject, input } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ButtonModule } from "primeng/button";
import { InputSwitchModule } from "primeng/inputswitch";
import { RadioButtonModule } from "primeng/radiobutton";
import { SidebarModule } from "primeng/sidebar";
import { ToggleButtonModule } from "primeng/togglebutton";
import { LayoutService } from "src/app/layout/services/layout.service";
import { MenuService } from "src/app/layout/services/menu.service";

@Component({
  selector: "app-config",
  standalone: true,
  templateUrl: "./app-config.component.html",
  imports: [CommonModule, FormsModule, SidebarModule, ButtonModule, RadioButtonModule, ToggleButtonModule, InputSwitchModule],
})
export class AppConfigComponent {
  readonly layoutService = inject(LayoutService);
  readonly menuService = inject(MenuService);

  readonly minimal = input<boolean>(false);

  scales: number[] = [12, 13, 14, 15, 16];

  get visible(): boolean {
    return this.layoutService.state.configSidebarVisible;
  }
  set visible(_val: boolean) {
    this.layoutService.state.configSidebarVisible = _val;
  }

  get scale(): number {
    return this.layoutService.config().scale;
  }
  set scale(val: number) {
    this.layoutService.config.update(config => ({
      ...config,
      scale: val,
    }));
  }

  get menuMode(): string {
    return this.layoutService.config().menuMode;
  }
  set menuMode(val: string) {
    this.layoutService.config.update(config => ({
      ...config,
      menuMode: val,
    }));
  }

  get inputStyle(): string {
    return this.layoutService.config().inputStyle;
  }
  set inputStyle(val: string) {
    this.layoutService.config.update(config => ({
      ...config,
      inputStyle: val,
    }));
  }

  get ripple(): boolean {
    return this.layoutService.config().ripple;
  }
  set ripple(val: boolean) {
    this.layoutService.config.update(config => ({
      ...config,
      ripple: val,
    }));
  }

  set theme(val: string) {
    this.layoutService.config.update(config => ({
      ...config,
      theme: val,
    }));
  }
  get theme(): string {
    return this.layoutService.config().theme;
  }

  set colorScheme(val: string) {
    this.layoutService.config.update(config => ({
      ...config,
      colorScheme: val,
    }));
  }
  get colorScheme(): string {
    return this.layoutService.config().colorScheme;
  }

  onConfigButtonClick() {
    this.layoutService.showConfigSidebar();
  }

  changeTheme(theme: string, colorScheme: string) {
    this.theme = theme;
    this.colorScheme = colorScheme;
  }

  decrementScale() {
    this.scale--;
  }

  incrementScale() {
    this.scale++;
  }
}
