import { Component, OnInit } from '@angular/core'
import * as moment from 'moment';
import { BookingService } from '../services/bookings-services/booking.service';
import { PrimeNGConfig, MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';
import { LoginService } from '../services/auth-services/login.service';
import { UserService } from '../services/user-services/user.service';
import { ApplicationService } from '../services/application.service';

@Component({
  templateUrl: './book.parking.space.component.html',
  styleUrls: ['./book.parking.space.component.scss'],
  providers: [MessageService]
})
export class BookParkingSpace implements OnInit {

  displayModal: boolean;
  allfreeSpaces: any;
  result: any;
  startDate: Date;
  endDate: Date;
  freeSpace: any;
  isParkingInfoFilled: boolean = true;
  en: any;

  constructor(private service: BookingService,
    private messageService: MessageService,
    private primengConfig: PrimeNGConfig,
    private loginService: LoginService,
    private userService: UserService,
    private applicationService: ApplicationService) { }

  ngOnInit() {
    this.primengConfig.ripple = true;
    this.en = this.applicationService.en;
    this.LoadAllFreeParkingSpaces();

    this.loginService.getCurrentUserData().subscribe(response => {
      if (response.PhoneNumber === null) {
        this.isParkingInfoFilled = false;
      }
    })
    this.userService.getUserInfo().subscribe(response => {
      if (response === null) {
        this.isParkingInfoFilled = false;
      }
      else if (response !== null && response.CarNumber === "") {
        this.isParkingInfoFilled = false;
      }
    })
  }

  book() { 
    let startDateOfBooking = this.Deserialize(this.result[0]);
    let endDateOfBooking = this.Deserialize(this.result[1]);
    let bookingModel = {
      StartDateString: startDateOfBooking,
      EndDateString: endDateOfBooking === "Invalid date" ? startDateOfBooking : endDateOfBooking,
      ParkingSpaceId: this.freeSpace.FreeParkingSpaceId
    };
    this.service.bookFreeSpace(bookingModel).subscribe((response: any) => {
      this.LoadAllFreeParkingSpaces();
      this.messageService.add({ severity: 'success', summary: `You booked parking space ${this.freeSpace.ParkingSpaceNumber} for the period ${response.StartDateString.split(' ')[0]} - ${response.EndDateString.split(' ')[0]}` });
    }, (err: HttpErrorResponse) => {
        this.messageService.add({
          severity: 'error', summary: `Space ${this.freeSpace.ParkingSpaceNumber} has already been booked.`
        });
        this.LoadAllFreeParkingSpaces();
    });
    
    this.displayModal = false;
  }

  close() {
    this.result = [];
    this.result = null;
    this.displayModal = false;
  }
  
  showModalDialog(space: any) {
    this.displayModal = true;
    this.freeSpace = space;
    let startDate = this.StringToDateConverter(space.StartDateString);
    this.endDate = this.StringToDateConverter(space.EndDateString);
    let today = new Date();
    if (startDate > today) {
      this.startDate = startDate;
    } else {
      this.startDate = today;
    }
  }

  StringToDateConverter(date: String): Date {
    let formattedDate = date.split('.').reverse().join('-');
    return new Date(formattedDate);
  }

  Deserialize(json: any): String {
    const value = moment(json, 'YYYY-MM-DD').toDate();
    var date = moment(json).format('DD/MM/YYYY');
    return date;
  }

  LoadAllFreeParkingSpaces() {
    this.service.getAllFreeParkingSpaces().subscribe(response => {
      response.map(x => {
        x.StartDateString = x.StartDateString.split(' ')[0];
        x.EndDateString = x.EndDateString.split(' ')[0];
      })
      this.allfreeSpaces = response;
    })
  }
}
