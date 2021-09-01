import { Component, OnInit } from '@angular/core';
import { LoginService } from '../services/auth-services/login.service';
import { FreeSpaceService } from '../services/free-spaces-services/free-space.service';
import { MessageService } from 'primeng/api';

@Component({
  templateUrl: './user.profile.component.html',
  styleUrls: ['./user.profile.component.scss'],
  providers: [MessageService]
})

export class UserProfile implements OnInit{
  parkingSpaceNumber: String = "";
  bookings: any = [];
  freeSpaces: any = [];

  constructor(private service: LoginService,
              private freeSpaceService: FreeSpaceService) { }

  ngOnInit() {
    this.service.getUserData().subscribe(response => {
      response.map(x => {
        x.StartDateString = x.StartDateString.split(' ')[0];
        x.EndDateString = x.EndDateString.split(' ')[0];
      });
      this.bookings = response;
    });

    this.service.getCurrentUserData().subscribe(response => {
      this.parkingSpaceNumber = response.ParkingSpaceNumber;
    });

    this.freeSpaceService.getAllFreeParkingSpacesForLoggedUser().subscribe(response => {
      response.map(x => {
        x.StartDateString = x.StartDateString.split(' ')[0];
        x.EndDateString = x.EndDateString.split(' ')[0];
      });
      this.freeSpaces = response;
    });
  }
}
