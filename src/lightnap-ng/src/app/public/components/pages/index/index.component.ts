import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { CardModule } from "primeng/card";

@Component({
  standalone: true,
  templateUrl: "./index.component.html",
  imports: [CommonModule, CardModule],
})
export class IndexComponent {

}
