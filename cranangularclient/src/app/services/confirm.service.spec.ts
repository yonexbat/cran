import { TestBed, inject, async } from '@angular/core/testing';
import { ConfirmService } from './confirm.service';
import {LanguageService} from './language.service';

describe('ConfirmService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LanguageService, ConfirmService]
    });
  });

  it('should be created', inject([ConfirmService], (service: ConfirmService) => {
    expect(service).toBeTruthy();
  }));

  it('should callback when ok', async(inject([ConfirmService], async (service: ConfirmService) => {
    const promise = service.confirm('test', 'testok');
    service.ok();
    try {
      await promise;
      expect(true).toBeTruthy();
    } catch (failure) {
      expect(false).toBeTruthy();
    }
  })));

  it('should callback when nok', async(inject([ConfirmService], async (service: ConfirmService) => {
    const promise = service.confirm('test', 'testnok');
    service.nok();
    try {
      await promise;
      expect(false).toBeTruthy();
    } catch (failure) {
      expect(true).toBeTruthy();
      expect(failure).toBe('cancel');
    }
  })));

});
