import { ComponentFixture, TestBed, tick, waitForAsync } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {ConfirmService} from '../../services/confirm.service';
import {LanguageService} from '../../services/language.service';
import { NotificationComponent } from './notification.component';
import {NotificationEvent, NotificationType} from '../../model/notificationEvent';

describe('NotificationComponent', () => {
  let component: NotificationComponent;
  let fixture: ComponentFixture<NotificationComponent>;

  beforeEach(waitForAsync(() => {
    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ NotificationComponent ],
      providers: [
        LanguageService, NotificationService,
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
        { provide: ConfirmService, useValue: confirmationService },
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('emit an error', waitForAsync(() => {
    const notificationService: NotificationService = fixture.debugElement.injector.get(NotificationService);
    notificationService.on().subscribe((res: NotificationEvent) => {
      fixture.whenStable().then(() => {
        fixture.detectChanges();

        const loader = fixture.nativeElement.querySelector('.loader');
        expect(loader).toBeFalsy('loading shoud not be visible');

        const error = fixture.nativeElement.querySelector('.alert-danger');
        expect(error).toBeTruthy('Errormessage shoud be showing');

        const textcontent = fixture.nativeElement.textContent;
        expect(textcontent).toContain('howdi');
      });
    });
    notificationService.emitError('howdi');
  }));

  it('emit an error without when stable', waitForAsync(() => {
    const notificationService: NotificationService = fixture.debugElement.injector.get(NotificationService);
    notificationService.on().subscribe((res: NotificationEvent) => {

        fixture.detectChanges();

        const loader = fixture.nativeElement.querySelector('.loader');
        expect(loader).toBeFalsy('loading shoud not be visible');

        const error = fixture.nativeElement.querySelector('.alert-danger');
        expect(error).toBeTruthy('Errormessage shoud be showing');

        const textcontent = fixture.nativeElement.textContent;
        expect(textcontent).toContain('howdi');

    });
    notificationService.emitError('howdi');
  }));

  it('emit a loading notificaton', waitForAsync(() => {
    const notificationService = fixture.debugElement.injector.get(NotificationService);
    notificationService.emitLoading();
    fixture.whenStable().then(() => {
      fixture.detectChanges();

      const loader = fixture.nativeElement.querySelector('.loader');
      expect(loader).toBeTruthy('loading shoud be visible');
    });
  }));
});
