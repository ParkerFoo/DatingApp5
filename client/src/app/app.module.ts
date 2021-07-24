import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

//added by FRS
import {HttpClientModule} from '@angular/common/http';
import { NavComponent } from './nav/nav.component'
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule, //aded by FRS 
    FormsModule, //added by FRS
    BrowserAnimationsModule, //added by FRS
    BsDropdownModule.forRoot() //added by FRS,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
