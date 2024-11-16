import { CommonModule } from "@angular/common";
import { Component, ContentChild, Input, OnChanges, SimpleChanges, TemplateRef } from "@angular/core";
import { ApiResponse, ErrorApiResponse, SuccessApiResponse } from "@core";
import { ProgressSpinnerModule } from "primeng/progressspinner";
import { catchError, map, Observable, of } from "rxjs";
import { ErrorListComponent } from "../error-list/error-list.component";

@Component({
  selector: "api-response",
  standalone: true,
  templateUrl: "./api-response.component.html",
  imports: [CommonModule, ErrorListComponent, ProgressSpinnerModule],
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
        catchError((error: ApiResponse<any>) => {
          if (!error.type) {
            console.error(`ApiResponseComponent expects an ApiResponse object to have been thrown in throwError, but received:`, error);
            throw Error("ApiResponseComponent expects an ApiResponse object to have been thrown in throwError");
          }

          if (error.errorMessages?.length > 0) {
            return of(error as ApiResponse<string>);
          }

          return of(new ErrorApiResponse<any>(["No error message provided"]));
        })
      );
    }
  }
}
