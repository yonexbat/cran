import { Injectable } from "@angular/core";
@Injectable()
export class ConfirmServiceSpy {
    public confirm(title: string, text: string): Promise<any> {
        return Promise.resolve();
    }
}
