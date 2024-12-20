import { NotificationData } from "./notification-data";

export interface LatestNotifications {
  unreadCount: number;
  notifications: Array<NotificationData>;
}
