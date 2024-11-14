import { CommonModule } from "@angular/common";
import { Component, ContentChild, Input, OnChanges, SimpleChanges, TemplateRef } from "@angular/core";
import { ApiResponse, ErrorApiResponse, SuccessApiResponse } from "@core";
import { catchError, map, Observable, of } from "rxjs";
import { ErrorListComponent } from "../error-list/error-list.component";

@Component({
  selector: "api-response",
  standalone: true,
  templateUrl: "./api-response.component.html",
  imports: [CommonModule, ErrorListComponent],
})
export class ApiResponseComponent implements OnChanges {
  @Input({ required: true }) apiResponse: Observable<any>;
  @Input() errorMessage = "An error occurred";
  @Input() loadingMessage = "Loading...";

  @ContentChild("success") successTemplateRef: TemplateRef<any>;
  @ContentChild("error") errorTemplateRef: TemplateRef<any>;
  @ContentChild("loading") loadingTemplateRef: TemplateRef<any>;

  internalApiResponse$?: Observable<ApiResponse<any>>;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["apiResponse"].currentValue) {
      this.internalApiResponse$ = this.apiResponse.pipe(
        map(result => new SuccessApiResponse(result)),
        catchError(error => of(error as ApiResponse<string>))
      );
    }
  }
}
