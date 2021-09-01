import { Routes } from '@angular/router'

import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { BookParkingSpace } from './book/book.parking.space.component';
import { SetFreeParkingSpace } from './set-free-space/set.free.space.component';
import { UserProfile } from './user-profile/user.profile.component';
import { AuthGuard } from '../../src/app/auth/auth.guard';
import { NavigationGuard } from '../../src/app/auth/navigation.guard';

export const appRoutes: Routes = [
  {
    path: '',
    component: LoginComponent,
    pathMatch: 'full'
  },
  
  {
    path: 'register',
    component: RegisterComponent
  },
  
  {
    path: 'profile',
    component: UserProfile,
    canActivate: [
      AuthGuard
    ]
  },
  {
    path: 'book',
    component: BookParkingSpace,
    canActivate: [
      AuthGuard
    ]
  },
  {
    path: 'free',
    component: SetFreeParkingSpace,
    canActivate: [
      AuthGuard
    ]
  },
];
