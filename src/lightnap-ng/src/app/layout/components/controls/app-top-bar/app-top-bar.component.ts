import { CommonModule } from "@angular/common";
import { Component, ElementRef, ViewChild } from "@angular/core";
import { RouterModule } from "@angular/router";
import { LayoutService } from "@layout/services/layout.service";
import { MenuItem } from "primeng/api";
import { StyleClassModule } from "primeng/styleclass";
import { AppConfiguratorComponent } from "../app-configurator/app-configurator.component";
import { NotificationsButtonComponent } from "@profile";
import { RoutePipe } from "@routing";

@Component({
  selector: "app-top-bar",
  standalone: true,
  templateUrl: "./app-top-bar.component.html",
  imports: [RouterModule, CommonModule, StyleClassModule, AppConfiguratorComponent, NotificationsButtonComponent, RoutePipe],
})
export class AppTopBarComponent {
    items!: MenuItem[];

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;

    constructor(public layoutService: LayoutService) {}

    toggleDarkMode() {
        this.layoutService.layoutConfig.update((state) => ({ ...state, darkTheme: !state.darkTheme }));
    }
}
