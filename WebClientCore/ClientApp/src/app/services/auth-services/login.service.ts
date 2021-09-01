import { Injectable } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICurrentUser } from '../../interfaces/auth-interfaces/current-user';
import { IBooking } from '../../interfaces/IBooking';
import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})

export class LoginService {

  baseUri: String = environment.BaseUri;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient
  ) { }

  login(formModel: any) {
    var body = {
      Name: formModel.value.name,
      Password: formModel.value.password
    }

    return this.http.post(this.baseUri + '/UserService.svc/Login', body);
  }

  getUserData(): Observable<Array<IBooking>> {
    return this.http.get<Array<IBooking>>(this.baseUri + '/BookingService.svc/AllBookingsByUser');
  }

  getCurrentUserData(): Observable<ICurrentUser> {
    return this.http.get<ICurrentUser>(this.baseUri + '/UserService.svc/CurrentUser');
  }
}
