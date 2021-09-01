import { Component, Input, OnInit } from '@angular/core';
import { BookingService } from '../../services/bookings-services/booking.service';
import { PrimeNGConfig, MessageService, SortEvent } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'bookings-table',
  templateUrl: './bookings.table.component.html',
  styleUrls: ['./bookings.table.component.scss'],
  providers: [MessageService]
})
export class BookingsTable implements OnInit{

  @Input() bookings: any[] = [];

  constructor(private service: BookingService,
              private messageService: MessageService,
              private primengConfig: PrimeNGConfig) { }

  ngOnInit() {
    this.primengConfig.ripple = true;
  }

  cancel(bookingId: any) {
    this.service.cancelBooking(bookingId).subscribe((response: any) => {
      this.bookings = this.bookings.filter(x => x.BookingId != bookingId);
      this.messageService.add({ severity: 'success', summary: 'You canceled your booking.' });
    }, (err: HttpErrorResponse) => {
      this.messageService.add({ severity: 'error', summary: 'Canceling your booking failed.', detail: 'Please try again.' });
    });
  }

  sortFn(event: SortEvent) {
    event.data.sort((data1, data2) => {
      if (event.field == "StartDateString") {
        var firstDateAsStringsArr = data1.StartDateString.split('.').reverse();
        var firstDate = new Date(firstDateAsStringsArr.join('-'));

        var secondDateAsStringsArr = data2.StartDateString.split('.').reverse();
        var secondDate = new Date(secondDateAsStringsArr.join('-'));
        var result = firstDate < secondDate ? -1 : (firstDate > secondDate ? 1 : 0);

        return (event.order * result);
      }
      else if (event.field == "EndDateString") {
        var firstDateAsStringsArr = data1.EndDateString.split('.').reverse();
        var firstDate = new Date(firstDateAsStringsArr.join('-'));

        var secondDateAsStringsArr = data2.EndDateString.split('.').reverse();
        var secondDate = new Date(secondDateAsStringsArr.join('-'));
        var result = firstDate < secondDate ? -1 : (firstDate > secondDate ? 1 : 0);

        return (event.order * result);
      }
      else if (event.field == "ParkingSpaceNumber") {
        var result = data1.ParkingSpaceNumber < data2.ParkingSpaceNumber ? -1 : (data1.ParkingSpaceNumber > data2.ParkingSpaceNumber ? 1 : 0);
        return (event.order * result);
      }
      else if (event.field == "Days"){
        var result = data1.Days < data2.Days ? -1 : (data1.Days > data2.Days ? 1 : 0);
        return (event.order * result);
      }
      else if (event.field == "MonthlyFee") {
        var result = data1.MonthlyFee < data2.MonthlyFee ? -1 : (data1.MonthlyFee > data2.MonthlyFee ? 1 : 0);
        return (event.order * result);
      }
      else if (event.field == "DueAmount") {
        var result = data1.DueAmount < data2.DueAmount ? -1 : (data1.DueAmount > data2.DueAmount ? 1 : 0);
        return (event.order * result);
      }
    });
  }

  isCancelButtonDisabled(date) {
    var dateParts = date.split(".");
    var startDate: Date = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
    var today: Date = new Date();
    if (startDate < today) {
      return true;
    }
    else {
      return false;
    }
  }
}
