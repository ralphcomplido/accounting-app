import { PagedResponse } from "@core";
import { Notification } from "./notification";

/**
 * Represents a paged response of notifications.
 * @extends PagedResponse<Notification>
 * @property {number} unreadCount - The number of unread notifications.
 */
export interface NotificationSearchResults extends PagedResponse<Notification> {
  unreadCount: number;
}
