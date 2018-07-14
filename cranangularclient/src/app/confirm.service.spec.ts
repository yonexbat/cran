import { TestBed, inject } from '@angular/core/testing';
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
});
