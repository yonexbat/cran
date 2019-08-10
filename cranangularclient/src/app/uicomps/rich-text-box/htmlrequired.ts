import {AbstractControl} from '@angular/forms';
export function htmlRequired(required: boolean) {
    return function validateHtmlRequired(c: AbstractControl): {[key: string]: any} | null  {
        if (required) {
            if (c.value === '') {
                return {
                    requiredError: 'field is required',
                };
            }
        }
        return null;
    };
}
