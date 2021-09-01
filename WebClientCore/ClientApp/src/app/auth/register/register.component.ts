import { Component, OnInit, Injectable } from '@angular/core';
import { RegisterService } from '../../services/auth-services/register.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { RegisterValidators } from '../../validators/register.validator';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: [
    './register.component.css',
    '../styles.scss'
  ]
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  usernameTaken: Boolean = false;

  constructor(private fb: FormBuilder, public service: RegisterService, private router: Router) { }

  ngOnInit() {
    this.registerForm = this.fb.group({
      name: ['', [RegisterValidators.usernameValidator, Validators.required]],
      passwordGroup: this.fb.group({
        password: ['', [RegisterValidators.passwordValidator, Validators.required]],
        confirmPassword: ['', [Validators.required]]
      }, { validator: RegisterValidators.passwordMatcher })
    }, { updateOn: 'blur' })

    this.registerForm.reset();
  }

  onSubmit() {
    this.service.register(this.registerForm).subscribe((response: any) => {
      if (response.succeeded) {
        setTimeout(() => { this.router.navigate(['']) }, 500);
      }
    }, (err: HttpErrorResponse) => {
      if (err.error === "Username is already taken.") {
        this.usernameTaken = true;
        this.registerForm.reset();
      }
    }, () => {
      setTimeout(() => { this.router.navigate(['']) }, 500);
    })
  }
}
