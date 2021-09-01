import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from "../../../environments/environment.prod";
import { IFree } from "../../interfaces/IFree";
import { IUserInfo } from "../../interfaces/IUserInfo";

@Injectable({
  providedIn: 'root'
})

export class UserService {
  baseUri: String = environment.BaseUri;

  constructor(private http: HttpClient) { }

  getUserInfo(): Observable<IUserInfo> {
    return this.http.get<IUserInfo>(this.baseUri + '/UserService.svc/UserInfo');
  }

  setUserInfo(userInfo: IUserInfo) {
    return this.http.post(this.baseUri + '/UserService.svc/SetUserInfo', userInfo);
  }
}
