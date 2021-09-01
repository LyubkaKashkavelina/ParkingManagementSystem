import { Component, Input, OnInit } from '@angular/core';
import { IFree } from '../../interfaces/IFree';
import { FreeSpaceService } from '../../services/free-spaces-services/free-space.service';
import { PrimeNGConfig, MessageService, SortEvent } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'free-spaces-table',
  templateUrl: './free.spaces.table.component.html',
  styleUrls: ['./free.spaces.table.component.scss'],
  providers: [MessageService]
})
export class FreeSpacesTable implements OnInit {

  @Input() freeSpaces: any[] = [];
  currentRoute: any = "";

  constructor(private service: FreeSpaceService,
              private messageService: MessageService,
              private primengConfig: PrimeNGConfig,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.primengConfig.ripple = true;
    this.route.url.subscribe(
      url => this.currentRoute = url[0].path
    );
  }

  cancel(freeParkingSpace: any) {
    this.service.cancelFreeSpace(freeParkingSpace.FreeParkingSpaceId).subscribe((response: any) => {
      this.freeSpaces = this.freeSpaces.filter(x => x.FreeParkingSpaceId != freeParkingSpace.FreeParkingSpaceId);
      this.messageService.add({ severity: 'success', summary: `Your parking space is no longer available for booking for the period ${freeParkingSpace.StartDateString} - ${freeParkingSpace.EndDateString}.` });
      setTimeout(() => {
        window.location.reload();
      }, 2000);
    }, (error: HttpErrorResponse) => {
        this.messageService.add({ severity: 'error', summary: 'Canceling your free space failed.', detail: 'Please try again.' });
    })
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
    });
  }

}
