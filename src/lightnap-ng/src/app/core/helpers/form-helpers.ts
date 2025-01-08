import { AbstractControl, FormGroup } from "@angular/forms";
import { throwInlineError } from "./error-helpers";

/**
 * Validator function to check if two password fields match.
 *
 * @param firstPasswordName - The name of the first password form control.
 * @param secondPasswordName - The name of the second password form control.
 * @returns A validator function that takes a FormGroup and returns an error object if the passwords do not match, or null if they do.
 */
export function confirmPasswordValidator(firstPasswordName: string, secondPasswordName: string) {
  return (form: AbstractControl) => {
    const newPassword = form.get(firstPasswordName) ?? throwInlineError(`Form control '${firstPasswordName}' not found.`);
    const confirmPassword = form.get(secondPasswordName) ?? throwInlineError(`Form control '${secondPasswordName}' not found.`);

    if (newPassword.value !== confirmPassword.value) {
      return { passwordsDoNotMatch: true };
    }

    return null;
  };
}
