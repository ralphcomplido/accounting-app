import { Component } from "@angular/core";
import { RouterModule } from "@angular/router";
import { IdentityCardComponent } from "@identity/components/controls/identity-card/identity-card.component";
import { RoutePipe } from "@routing";

@Component({
  standalone: true,
  templateUrl: "./reset-instructions-sent.component.html",
  imports: [RouterModule, RoutePipe, IdentityCardComponent],
})
export class ResetInstructionsSentComponent {}
