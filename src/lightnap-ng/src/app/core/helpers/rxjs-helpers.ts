import { ErrorApiResponse } from "@core/models";
import { distinctUntilChanged, Observable, throwError } from "rxjs";

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
 * Wraps throwError by creating an appropriate ErrorApiResponse object while casting the return to the requested type.
 *
 * @template T The type of items the calling pipeline operator needs to return.
 * @param {string | Array<string>} error - The single or array of user-friendly error messages.
 * @param {@template T} referenceObject - Optional argument to simplify defining the type of this method.
 * @returns A function that returns an Observable that emits ApiResponse objects.
 */
export function throwErrorApiResponse<T>(error: string | Array<string>, referenceObject: T | undefined = null) {
  return throwError(() => new ErrorApiResponse<T>(Array.isArray(error) ? error : [error])) as Observable<T>;
}
