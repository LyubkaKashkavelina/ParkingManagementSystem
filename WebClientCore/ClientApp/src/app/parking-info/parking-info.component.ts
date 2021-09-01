import { Component, OnInit } from '@angular/core'
import { PrimeNGConfig, MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';
import { IUserInfo } from '../interfaces/IUserInfo';
import { UserService } from '../services/user-services/user.service';
import { LoginService } from '../services/auth-services/login.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'parking-info',
  templateUrl: './parking-info.component.html',
  styleUrls: ['./parking-info.component.scss'],
  providers: [MessageService]
})
export class ParkingInfo implements OnInit {
  parkingSpaceNumber: String = "";
  bookedParkingSpaceForToday: String = "";
  userInfo: IUserInfo = <IUserInfo>{};
  displayModal: boolean;
  editForm: FormGroup;

  constructor(private service: LoginService,
    private messageService: MessageService,
    private primengConfig: PrimeNGConfig,
    private userService: UserService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.primengConfig.ripple = true;

    this.editForm = this.fb.group({
      CarNumber: ['', [Validators.maxLength(16), Validators.required]],
      Make: ['', [Validators.maxLength(64)]],
      Model: ['', [Validators.maxLength(64)]],
      Color: ['', [Validators.maxLength(64)]],
      PhoneNumber: ['', [Validators.maxLength(32), Validators.required]]
    }, { updateOn: 'change' })

    this.service.getCurrentUserData().subscribe(response => {
      this.parkingSpaceNumber = response.ParkingSpaceNumber;
      this.bookedParkingSpaceForToday = response.BookedParkingSpaceForToday;
      this.userInfo.PhoneNumber = response.PhoneNumber;

      this.editForm.patchValue({
        PhoneNumber: response.PhoneNumber
      })
    })

    this.userService.getUserInfo().subscribe(response => {
      if (response !== null) {
        this.userInfo.CarNumber = response.CarNumber;
          this.userInfo.Make = response.Make;
          this.userInfo.Model = response.Model;
          this.userInfo.Color = response.Color;

          this.editForm.patchValue({
            CarNumber: response.CarNumber,
            Make: response.Make,
            Model: response.Model,
            Color: response.Color
          })
      }
      
    })
  }

  showModalDialog() {
    this.displayModal = true;
  }

  edit() {
    this.userService.setUserInfo(this.editForm.value).subscribe((response: IUserInfo) => {
      this.userInfo = response;
      this.messageService.add({ severity: 'success', summary: 'You edited your parking info.' });
    }, (err: HttpErrorResponse) => {
      this.messageService.add({ severity: 'error', summary: 'Editing your parking info failed.', detail: 'Please try again.' });
    })
    this.displayModal = false;
  }

  close() {
    this.displayModal = false;

    this.editForm.setValue({
      CarNumber: this.userInfo.CarNumber,
      Make: this.userInfo.Make,
      Model: this.userInfo.Model,
      Color: this.userInfo.Color,
      PhoneNumber: this.userInfo.PhoneNumber
    })
  }
}
