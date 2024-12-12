/**
 * Represents a request to send a magic link email.
 */
export interface SendMagicLinkEmailRequest {
    /**
     * The email address associated with the user's account.
     */
    email: string;
}
