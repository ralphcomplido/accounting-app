import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { RouterLink } from "@angular/router";
import { RoutePipe } from "@routing";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";

@Component({
  standalone: true,
  templateUrl: './change-email-requested.component.html',
  imports: [CommonModule, CardModule, ButtonModule, RouterLink, RoutePipe],
})
export class ChangeEmailRequestedComponent {
}
