import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { CranDataService } from './cran-data.service';
import { PagedResult } from '../model/pagedresult';
import { Course } from '../model/course';
import { createCoursesTestObjs } from '../testing/modelobjcreator';

describe('LanguageService', () => {

  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CranDataService],
    });

    httpClient = TestBed.get(HttpClient);
    httpTestingController = TestBed.get(HttpTestingController);
  });

  afterEach(() => {
    // After every test, assert that there are no more pending requests.
    httpTestingController.verify();
  });

  it('should be created', inject([CranDataService], (service: CranDataService) => {
    expect(service).toBeTruthy();
  }));

  it('should call httpclient', inject([CranDataService], (service: CranDataService) => {
    const testCourses =  createCoursesTestObjs();
    const restResult: PagedResult<Course> = new PagedResult<Course>();
    restResult.count = 1;
    restResult.numpages = 1;
    restResult.currentPage = 0;
    restResult.data = testCourses;

    service.getCourses(0).then((data) => {
        expect(data).toBeTruthy();
        expect(data.count).toBe(1);
        expect(data.data[0].title).toBe(testCourses[0].title, 'Title of first course expected to be angular');
    });
    const req = httpTestingController.expectOne('/api/Data/GetCourses/0');
    expect(req.request.method).toEqual('GET');

    // Respond with the mock heroes
    req.flush(restResult);
  }));

});
