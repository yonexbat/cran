import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { CourseListComponent } from './course-list/course-list.component';
import { CranDataService } from './cran-data.service';
import { AppRoutingModule } from './app-routing/app-routing.module';
import { HomeComponent } from './home/home.component';
import { ManageQuestionComponent } from './manage-question/manage-question.component';

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    CourseListComponent,
    HomeComponent,
    ManageQuestionComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpModule,
    FormsModule,
    AppRoutingModule,
  ],
  providers: [
    CranDataService,
    HttpModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
