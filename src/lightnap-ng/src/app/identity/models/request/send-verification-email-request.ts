/**
 * Represents a request to send a verification email.
 */
export interface SendVerificationEmailRequest {
    /**
     * The email address associated with the user's account.
     */
    email: string;
}
