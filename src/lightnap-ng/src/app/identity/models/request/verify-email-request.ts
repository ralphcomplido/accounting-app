/**
 * Represents a request to verify an email.
 */
export interface VerifyEmailRequest {
    /**
     * The email address associated with the verification request.
     */
    email: string;

    /**
     * The verification code to be validated.
     */
    code: string;
}
