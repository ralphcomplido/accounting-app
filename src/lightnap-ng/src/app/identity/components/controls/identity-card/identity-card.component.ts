import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { LayoutService } from 'src/app/layout/services/layout.service';

@Component({
    selector: 'identity-card',
    standalone: true,
    templateUrl: './identity-card.component.html',
    imports: [CommonModule]
})
export class IdentityCardComponent {
    layoutService = inject(LayoutService);
}
