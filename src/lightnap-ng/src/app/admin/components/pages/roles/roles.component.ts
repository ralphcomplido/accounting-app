import { AdminService } from '@admin/services/admin.service';
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiResponseComponent } from '@core/components/controls/api-response/api-response.component';
import { RoutePipe } from '@routing';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';

@Component({
    standalone: true,
    templateUrl: './roles.component.html',
    imports: [CommonModule, CardModule, RouterLink, RoutePipe, ApiResponseComponent, TableModule]
})
export class RolesComponent {
    #adminService = inject(AdminService);

    roles$ = this.#adminService.getRoles();
}
