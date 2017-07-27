import { Component } from '@angular/core';
import {CranDataServiceService} from './cran-data-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [
    CranDataServiceService,
  ]
})
export class AppComponent {
  title = 'app';
}
