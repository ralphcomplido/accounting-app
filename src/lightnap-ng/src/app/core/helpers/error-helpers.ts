/**
 * Throws an inline error with the specified message. This is intended to be used inline with code for things like validating inputs.
 *
 * @param message - The error message to be thrown.
 * @throws {Error} Throws an error with the provided message.
 * @example
 * const value = environment.value ?? throwInlineError("Required setting 'environment.value' is not defined.")
 */
export function throwInlineError(message: string) {
  return (() => {
    throw new Error(message);
  })();
}
