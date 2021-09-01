import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { IBooking } from "../../interfaces/IBooking";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class BookingService {
  baseUri: String = environment.BaseUri;

  constructor(private http: HttpClient) {

  }

  bookFreeSpace(booking: IBooking) {
    return this.http.post(this.baseUri + '/BookingService.svc/BookFreeParkingSpace', booking);
  }

  getAllFreeParkingSpaces(): Observable<any> {
    return this.http.get<any>(this.baseUri + '/BookingService.svc/AllFreeParkingSpaces');
  }

  cancelBooking(bookingId: any) {
    var body = {
      BookingId: bookingId
    };
    return this.http.post(this.baseUri + '/BookingService.svc/CancelBooking', body);
  }
}

