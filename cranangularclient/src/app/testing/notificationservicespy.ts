import { Injectable } from "@angular/core";
@Injectable()
export class NotificationServiceSpy {
    public emitError(message: string): void {
    }
    public emitLoading(): void {
    }
    public emitDone(): void {
    }
}
