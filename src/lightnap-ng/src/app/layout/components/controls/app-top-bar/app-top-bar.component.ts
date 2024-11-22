import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { RoutePipe } from '@routing';
import { LayoutService } from 'src/app/layout/services/layout.service';

@Component({
    selector: 'app-top-bar',
    standalone: true,
    templateUrl: './app-top-bar.component.html',
    imports: [CommonModule, RouterModule, ButtonModule, RippleModule, RoutePipe]
})
export class AppTopBarComponent {
    layoutService = inject(LayoutService);

    items!: MenuItem[];

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;
}
