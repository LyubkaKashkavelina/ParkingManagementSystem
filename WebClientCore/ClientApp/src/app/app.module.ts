import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { BookParkingSpace } from './book/book.parking.space.component';
import { SetFreeParkingSpace } from './set-free-space/set.free.space.component';
import { UserProfile } from './user-profile/user.profile.component';
import { BookingsTable } from './tables/bookings-table/bookings.table.component';
import { FreeSpacesTable } from './tables/free-spaces-table/free.spaces.table.component';
import { AllFreeSpacesTable } from './tables/all-free-spaces-table/all.free.spaces.table.component';
import { ParkingInfo } from './parking-info/parking-info.component'

import { appRoutes } from './routes';
import { CommonModule } from '@angular/common';
import { LoginService } from './services/auth-services/login.service';
import { AuthInterceptor } from './auth/auth.interceptor';

import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { RippleModule } from 'primeng/ripple';
import { MessageService } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    RegisterComponent,
    LoginComponent,
    BookParkingSpace,
    SetFreeParkingSpace,
    UserProfile,
    BookingsTable,
    FreeSpacesTable,
    AllFreeSpacesTable,
    ParkingInfo
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    RouterModule.forRoot(appRoutes),
    CalendarModule,
    BrowserAnimationsModule,
    DialogModule,
    TableModule,
    ButtonModule,
    ToastModule,
    RippleModule,
    MenubarModule
  ],
  providers: [
    LoginService, {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
