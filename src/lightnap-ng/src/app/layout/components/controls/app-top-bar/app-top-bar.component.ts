import { CommonModule } from "@angular/common";
import { Component, ElementRef, inject, viewChild } from "@angular/core";
import { RouterModule } from "@angular/router";
import { NotificationsButtonComponent } from "@profile/components/controls/notifications-button/notifications-button.component";
import { RoutePipe } from "@routing";
import { MenuItem } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { RippleModule } from "primeng/ripple";
import { LayoutService } from "src/app/layout/services/layout.service";

@Component({
  selector: "app-top-bar",
  standalone: true,
  templateUrl: "./app-top-bar.component.html",
  imports: [CommonModule, RouterModule, ButtonModule, RoutePipe, NotificationsButtonComponent, RippleModule, RoutePipe],
})
export class AppTopBarComponent {
  readonly layoutService = inject(LayoutService);

  readonly menuButton = viewChild.required<ElementRef>("menubutton");
  readonly topbarMenuButton = viewChild.required<ElementRef>("topbarmenubutton");
  readonly menu = viewChild.required<ElementRef>("topbarmenu");

  readonly items!: MenuItem[];
}
