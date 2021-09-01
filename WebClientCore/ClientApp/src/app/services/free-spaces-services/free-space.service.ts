import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from "../../../environments/environment.prod";
import { IFree } from "../../interfaces/IFree";

@Injectable({
  providedIn: 'root'
})

export class FreeSpaceService {
  baseUri: String = environment.BaseUri;

  constructor(private http: HttpClient) { }

  setParkingSpaceAsFree(startDate: String, endDate: String) {
    var body = {
      StartDateString: startDate,
      EndDateString: endDate
    };
    return this.http.post(this.baseUri + '/BookingService.svc/SetFreeParkingSpace', body); 
  }

  getAllFreeParkingSpacesForLoggedUser(): Observable<Array<IFree>> {
    return this.http.get<Array<IFree>>(this.baseUri + '/BookingService.svc/AllFreeParkingSpacesByUser');
  }

  cancelFreeSpace(freeParkingSpaceId: any) {
    var body = {
      FreeParkingSpaceId: freeParkingSpaceId
    };
    return this.http.post(this.baseUri + '/BookingService.svc/CancelFreeParkingSpace', body);
  }

}
