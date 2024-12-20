export interface NotificationData {
  id: number;
  timestamp: Date;
  isUnread: boolean;
  title: string;
  description: string;
  routerLink: Array<string>;
}
