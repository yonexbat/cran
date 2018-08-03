import { TestBed,  fakeAsync, tick } from '@angular/core/testing';
import {asyncData, asyncError} from './async';
import { of, Observable, Subject, Subscriber, Subscription } from 'rxjs';
import { cold, getTestScheduler } from 'jasmine-marbles';


class CraniumService {

  private observableIntern = new Subject<string>();

  public getWordOfDay(): Observable<string> {
     return this.observableIntern;
  }

  public pushMessage(message: string) {
    this.observableIntern.next(message);
  }

  public done() {
    this.observableIntern.complete();
  }

  public pushError(errorMessage: string) {
    this.observableIntern.error(new Error(errorMessage));
  }
}

describe('Marble test', () => {

  const serviceReturnValue = 'cranium';
  let craniumService;
  let getWordOfDaySpy;

  beforeEach(() => {

    craniumService = jasmine.createSpyObj('CraniumService', ['getWordOfDay']);
    getWordOfDaySpy = craniumService.getWordOfDay.and.returnValue(of(serviceReturnValue));

    TestBed.configureTestingModule({
      providers: []
    });
  });

  it('test realservice synchronous', () => {
    const synchronousCraniumService = new CraniumService();
    const subsciption = synchronousCraniumService.getWordOfDay().subscribe(
    (message: string) => {
      expect(message).toBe('hello');
    },
    (error) => {},
    () => {});
    synchronousCraniumService.pushMessage('hello');
    subsciption.unsubscribe();
    synchronousCraniumService.done();
  });

  it('test realservice error', () => {
    const synchronousCraniumService = new CraniumService();

    let message: string;
    let error;
    const subsciption = synchronousCraniumService.getWordOfDay().subscribe(
    (thismessage: string) => {
      message = thismessage;
    },
    (thiserror) => {
      error = thiserror;
    },
    () => {});
    synchronousCraniumService.pushError('hello error');
    synchronousCraniumService.pushMessage('mymessage');
    expect(message).toBeUndefined();
    expect(error).toBeDefined();
    subsciption.unsubscribe();
    synchronousCraniumService.done();
  });

  it('test spyservice syncrhonous', () => {
    let itiscalled: string;
    const subsciption = craniumService.getWordOfDay().subscribe(
      (message: string) => {
        itiscalled = message;
      },
      (error) => {},
      () => {}
    );

    expect(itiscalled).toBe(serviceReturnValue);
    subsciption.unsubscribe();
  });

  it('test fakeasync ok', fakeAsync(() => {
    let itiscalled: string;
    getWordOfDaySpy.and.returnValue(asyncData(serviceReturnValue));

    const subsciption = craniumService.getWordOfDay().subscribe(
      (message: string) => {
        itiscalled = message;
      },
      (error) => {},
      () => {}
    );
    expect(itiscalled).toBeUndefined();
    tick();
    expect(itiscalled).toBe(serviceReturnValue);
    subsciption.unsubscribe();
  }));

  it('test fakeasync error', fakeAsync(() => {
    let errorOccurred = false;
    getWordOfDaySpy.and.returnValue(asyncError(serviceReturnValue));

    const subsciption = craniumService.getWordOfDay().subscribe(
      (message: string) => { },
      (error) => {
        errorOccurred = true;
      },
      () => {}
    );
    expect(errorOccurred).toBeFalsy();
    tick();
    expect(errorOccurred).toBeTruthy();
    subsciption.unsubscribe();
  }));

  it('test marbel cold', () => {
    let numcalls = 0;
    let numerrors = 0;
    const q$ = cold('-x----x--x--y--z-#|', { x: serviceReturnValue, y: 'hello', z: 'from marble' },
      new Error('CraniumService failure'));
    getWordOfDaySpy.and.returnValue( q$ );

    const subsciption = craniumService.getWordOfDay().subscribe(
      (message: string) => {
        numcalls++;
      },
      (error) => {
        numerrors++;
      },
      () => {}
    );

    getTestScheduler().flush();
    expect(numcalls).toBe(5);
    expect(numerrors).toBe(1);
    subsciption.unsubscribe();
  });
});

describe('Observable tests', () => {
  it('observable unsibscribe when resolved', () => {
    let unsubscribeCalled = false;
    const observable = new Observable<Number>((observer: Subscriber<Number>) => {
      observer.next(1);
      observer.next(2);
      observer.next(3);
      observer.complete();
      return () => {
        unsubscribeCalled = true;
      };
    });

    let x = 0;
    let complete = false;
    const subsciption: Subscription = observable.subscribe(
      // next
      (thenumber: number) => {
        x++;
      },

      // error
      (theerror) => {},

      // complete
      () => {
        complete = true;
      }
    );
    expect(x).toBe(3);
    expect(complete).toBe(true);
    expect(subsciption.closed).toBe(true);
    const unsubscribeFn = subsciption.unsubscribe;
    expect(unsubscribeFn).toBeDefined();
    subsciption.unsubscribe();
    expect(unsubscribeCalled).toBeTruthy();

  });

  it('observable unsibscribe when not resolved', () => {
    const observable = new Observable<Number>((observer: Subscriber<Number>) => {
      observer.next(1);
      observer.next(2);
      observer.next(3);
      return () => {};
    });

    let x = 0;
    let complete = false;
    const subsciption: Subscription = observable.subscribe(
      // next
      (thenumber: number) => {
        x++;
      },

      // error
      (theerror) => {},

      // complete
      () => {
        complete = true;
      }
    );
    expect(x).toBe(3);
    expect(complete).toBe(false, 'completed to be false' );
    expect(subsciption.closed).toBe(false, 'subscription to be still open');
    const unsubscribeFn = subsciption.unsubscribe;
    expect(unsubscribeFn).toBeDefined();
    subsciption.unsubscribe();

  });
});
