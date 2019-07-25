import { Injectable, InjectionToken, Injector  } from '@angular/core';

describe('Di tests', () => {

    it('test multiple', () => {
        const TOKEN = new InjectionToken<string[]>('Testoken');

        const injector = Injector.create([
        { provide: TOKEN, useValue: 'dependency one', multi: true },
        { provide: TOKEN, useValue: 'dependency two', multi: true }
        ]);

        const dependencies: string[] = injector.get(TOKEN);
        expect(dependencies.length).toBe(2, 'array with two entries');
    });
});
