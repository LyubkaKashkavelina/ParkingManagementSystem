import { Component, Input, OnInit, EventEmitter, Output } from '@angular/core';
import { FreeSpaceService } from '../../services/free-spaces-services/free-space.service';
import { PrimeNGConfig, MessageService, SortEvent } from 'primeng/api';

@Component({
  selector: 'all-free-spaces-table',
  templateUrl: './all.free.spaces.table.component.html',
  styleUrls: ['./all.free.spaces.table.component.scss'],
  providers: [MessageService]
})
export class AllFreeSpacesTable implements OnInit {

  @Input() freeSpaces: any[] = [];
  @Input() isParkingInfoFilled: boolean;
  @Output() public emitData = new EventEmitter();

  constructor(private service: FreeSpaceService,
    private messageService: MessageService,
    private primengConfig: PrimeNGConfig) { }

  ngOnInit() {
    this.primengConfig.ripple = true;
  }

  book(data) {
    this.emitData.emit(data);
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
    });
  }

}

