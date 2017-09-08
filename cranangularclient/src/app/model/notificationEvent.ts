export class NotificationEvent {
    message: string;
    type: NotificationType;
}
export enum NotificationType {
    Error = 1,
    Warn = 2,
    Done = 3,
    Loading = 4,
}
