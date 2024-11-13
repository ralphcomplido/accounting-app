import { ApiResponse } from "@core/models";
import { catchError, distinctUntilChanged, mergeMap, Observable, of, OperatorFunction, switchMap, throwError } from "rxjs";

/**
 * RxJS operator that filters out items emitted by the source Observable that are the same as the previous item when stringified.
 *
 * @template T The type of items emitted by the source Observable.
 * @returns A function that returns an Observable that emits items from the source Observable only when the serialized JSON representation of the current item is different from the serialized JSON representation of the previous item.
 */
export function distinctUntilJsonChanged<T>() {
  return (source$: Observable<T>) =>
    source$.pipe(distinctUntilChanged((original, incoming) => JSON.stringify(original) === JSON.stringify(incoming)));
}

/**
 * Suppresses API errors by throwing unsuccessful responses via RxJS throwError(). This ensures that only successful responses
 * make it to the next operator. As a result, response.result can be assumed to be the valid response. However, it keep in mind
 * it can still be null or undefined depending on the API call (like a get for something that doesn't exist).
 *
 * If the response type is not "Success", it uses the RxJS throwError() to throw the response. It's highly recommended to use
 * restoreApiError() at the end of the pipeline in order to restore a final error response matching the same type as a successful
 * pipeline (which should be your final emitted type) if you're in the middle of a pipeline.
 *
 * Otherwise you can set up a subscription catch handler. This is preferred when you're at the end of a pipeline and you want to
 * conveniently split actions.
 *
 * @typeParam T - The T of the most recent ApiResponse<T> produced by the previous operator. This ensures the result is of the same type.
 * @returns An operator function that transforms the source observable.
 */
export function throwIfApiError<T>(): OperatorFunction<ApiResponse<T>, ApiResponse<T>> {
  return (source: Observable<ApiResponse<T>>) =>
    source.pipe(
      switchMap(response => {
        if (response.type !== "Success") {
          return throwError(() => response);
        }
        return of(response);
      })
    );
}

/**
 * Catches any error thrown earlier by throwIfApiError. This is useful for restoring the final error response to the same type as
 * the successful response when you are not the one subscribing. If you don't use this after throwIfApiError() then you need to set
 * set up an error handler in your subscription.
 *
 * @typeParam T - The T of the final ApiResponse<T> produced by the previous operator. This ensures the result is of the same type.
 * @returns An operator function that transforms the source observable.
 */
export function catchApiError<T>(): OperatorFunction<ApiResponse<T>, ApiResponse<T>> {
  return (source: Observable<ApiResponse<T>>) => source.pipe(catchError(response => of(response as any as ApiResponse<T>)));
}
