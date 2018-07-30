export class ConfirmServiceSpy {
    public confirm(title: string, text: string): Promise<any> {
        return Promise.resolve();
    }
}
