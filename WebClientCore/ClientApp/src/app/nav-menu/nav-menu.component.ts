import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../services/auth-services/login.service';
import { MenuItem } from 'primeng/api/menuitem';

@Component({
  selector: 'nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit{

  username: String;
  hasPersonalParkingSpace: Boolean;
  items: MenuItem[];

  constructor(private router: Router, private service: LoginService) {}

  ngOnInit() {
    this.service.getCurrentUserData().subscribe(response => {
      this.username = response.Name;
      this.hasPersonalParkingSpace = response.ParkingSpaceNumber ? true : false;

      if (this.hasPersonalParkingSpace) {
        this.items = [
          { label: 'Home', icon: 'pi pi-home', routerLink: ['/profile'], styleClass: this.isOnHomePage() ? "nav-links nav-link-active" : "nav-links" },
          { label: 'Book', icon: 'pi pi-calendar', routerLink: ['/book'], styleClass: this.isOnBookPage() ? "nav-links nav-link-active" : "nav-links" },
          { label: 'My parking space', icon: 'pi pi-check', routerLink: ['/free'], styleClass: this.isOnMySpacePage() ? "nav-links nav-link-active" : "nav-links" },
          { label: `<p>Welcome,<span>${this.username}</span></p>`, escape: false, styleClass: "welcome-message" },
          { label: '<a routerLinkActive="active" class="logout-link">Logout</a>', escape: false, command: () => this.logOut(), styleClass: "logout-button" }
        ];
      }
      else {
        this.items = [
          { label: 'Home', icon: 'pi pi-home', routerLink: ['/profile'], styleClass: this.isOnHomePage() ? "nav-links nav-link-active" : "nav-links" },
          { label: 'Book', icon: 'pi pi-calendar', routerLink: ['/book'], styleClass: this.isOnBookPage() ? "nav-links nav-link-active" : "nav-links" },
          { label: `<p>Welcome,<span>${this.username}</span></p>`, escape: false, styleClass: "welcome-message" },
          { label: '<a routerLinkActive="active" class="logout-link">Logout</a>', escape: false, command: () => this.logOut(), styleClass: "logout-button"  }
        ];
      }
    })
  }

  isOnHomePage() {
    if (this.router.url === "/profile") {
      return true;
    }
    return false;
  }

  isOnBookPage() {
    if (this.router.url === "/book") {
      return true;
    }
    return false;
  }

  isOnMySpacePage() {
    if (this.router.url === "/free") {
      return true;
    }
    return false;
  }

  logOut() {
    localStorage.removeItem("token");
    setTimeout(() => {
      this.router.navigate([''])
    }, 500);
  }
}
