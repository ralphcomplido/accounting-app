import { PaginationRequest } from "@core";
import { NotificationStatus } from "../notifications/notification-status";
import { NotificationType } from "../notifications/notification-type";

/**
 * Interface representing a request to search for the user's notifications.
 * Extends the PaginationRequest interface to include pagination properties.
 */
export interface SearchNotificationsRequest extends PaginationRequest {
  /**
   * The ID to filter notifications since.
   * @type {number}
   */
  sinceId?: number;

  /**
   * The status of the notifications to filter by.
   * @type {NotificationStatus}
   */
  status?: NotificationStatus;

  /**
   * The type of the notifications to filter by.
   * @type {NotificationType}
   */
  type?: NotificationType;
}
