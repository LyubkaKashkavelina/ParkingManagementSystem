import { Component, OnInit } from '@angular/core'
import * as moment from 'moment';
import { FreeSpaceService } from '../services/free-spaces-services/free-space.service';
import { PrimeNGConfig, MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';
import { ApplicationService } from '../services/application.service';

@Component({
  templateUrl: './set.free.space.component.html',
  styleUrls: ['./set.free.space.component.scss'],
  providers: [MessageService]
})
export class SetFreeParkingSpace implements OnInit{
  displayModal: boolean;
  result: any;
  Today = new Date();
  unactiveDaysInCalendar = new Array<Date>();
  freeSpaces: any = [];
  freeParkingSpacesForLoggedUser: any;
  en: any;


  constructor(private service: FreeSpaceService,
    private messageService: MessageService,
    private primengConfig: PrimeNGConfig,
    private applicationService: ApplicationService) { }

  ngOnInit() {
    this.primengConfig.ripple = true;
    this.en = this.applicationService.en;

    this.service.getAllFreeParkingSpacesForLoggedUser().subscribe(response => {
      response.map(x => {
        x.StartDateString = x.StartDateString.split(' ')[0];
        x.EndDateString = x.EndDateString.split(' ')[0];
        this.unactiveDaysInCalendar = this.unactiveDaysInCalendar.concat(this.getAllDatesBetweenTwoDates(new Date(x.StartDateString.split('.').reverse().join('-')), new Date(x.EndDateString.split('.').reverse().join('-'))));
      });
      this.freeSpaces = response;
    })
  }

  sublet() {
    this.displayModal = false;
    let startDate = this.Deserialize(this.result[0]);
    let endDate = this.Deserialize(this.result[1]);
    endDate = endDate === "Invalid date" ? startDate : endDate;
    this.service.setParkingSpaceAsFree(startDate, endDate).subscribe((response: any) => {
      let freeSpace = {
        StartDateString: response.StartDateString.split(" ")[0],
        EndDateString: response.EndDateString.split(" ")[0],
        FreeParkingSpaceId: response.FreeParkingSpaceId
      } 
      this.freeSpaces.push(freeSpace);
      this.unactiveDaysInCalendar = this.unactiveDaysInCalendar.concat(this.getAllDatesBetweenTwoDates(new Date(freeSpace.StartDateString.split('.').reverse().join('-')), new Date(freeSpace.EndDateString.split('.').reverse().join('-'))));

      this.messageService.add({ severity: 'success', summary: `Your parking space is available for booking for the period ${response.StartDateString.split(' ')[0]} - ${response.EndDateString.split(' ')[0]}` });
    }, (err: HttpErrorResponse) => {
        this.messageService.add({ severity: 'error', summary: 'The parking space cannot be freed.' });
    });

    this.service.getAllFreeParkingSpacesForLoggedUser().subscribe(response => {
      this.freeParkingSpacesForLoggedUser = response;
    })
  }

  close() {
    this.result = [];
    this.result = null;
    this.displayModal = false;
  }

  Deserialize(json: any): String {
    const value = moment(json, 'YYYY-MM-DD').toDate();
    var date = moment(json).format('DD/MM/YYYY');
    return date;
  }

  DeserializeWithDots(json: any): String {
    const value = moment(json, 'YYYY-MM-DD').toDate();
    var date = moment(json).format('DD.MM.YYYY');
    return date;
  }

  showModalDialog() {
    this.displayModal = true;
  }

  getAllDatesBetweenTwoDates(startDate: Date, endDate: Date) {
    var datesArray = new Array<Date>();
    var currentDate: Date = startDate;
    while (currentDate <= endDate) {
      datesArray.push(new Date(currentDate));
      currentDate.setDate(currentDate.getDate() + 1);
    }
    return datesArray;
  }
}
