import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AstronautComponent } from './astronaut/astronaut.component';
import { AstronautDetailsComponent } from './astronaut-details/astronaut-details.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AstronautComponent,
    AstronautDetailsComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
