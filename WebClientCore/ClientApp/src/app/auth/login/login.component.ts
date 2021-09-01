import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LoginService } from '../../services/auth-services/login.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: [
    './login.component.css',
    '../styles.scss'
  ]
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  userNotFound: Boolean = false;

  constructor(private fb: FormBuilder, private service: LoginService, private router: Router) {}

  ngOnInit() {
    localStorage.removeItem("token");
    this.loginForm = this.fb.group({
      name: [''],
      password: ['']
    })
  }

  login() {
    this.service.login(this.loginForm).subscribe((response: any) => {
      localStorage.setItem('token', response);
    }, (err: HttpErrorResponse) => {
        this.userNotFound = true;
        this.loginForm.reset();
    }, () => {
        setTimeout(() => { this.router.navigate(['/profile']) }, 500);
      })
  }
}
