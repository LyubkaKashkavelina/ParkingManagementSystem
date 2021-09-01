import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})

export class RegisterService {

  baseUri: String = environment.BaseUri;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient
  ) { }

  register(formModel: any) {
    var body = {
      Name: formModel.value.name,
      Password: formModel.value.passwordGroup.password
    }

    return this.http.post(this.baseUri + '/UserService.svc/Register', body);
  }

}
