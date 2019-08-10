import { Injectable, InjectionToken  } from '@angular/core';
import { ICranDataService } from './icrandataservice';
export const CRAN_SERVICE_TOKEN = new InjectionToken<ICranDataService>('ICranDataService');
