import { Notification } from "@profile/models";

export class NotificationHelper {
    public static rehydrate(notification: Notification) {
        if (notification?.timestamp) {
            notification.timestamp = new Date(notification.timestamp);
        }
    }
}
