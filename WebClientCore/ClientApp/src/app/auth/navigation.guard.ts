import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { LoginService } from '../services/auth-services/login.service';

@Injectable({
  providedIn: 'root'
})
export class NavigationGuard {

  constructor(private router: Router,
              private service: LoginService) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    this.service.getCurrentUserData().subscribe(response => {
      if (response?.ParkingSpaceNumber) {
        return true;
      }
      else {
        this.router.navigate(['/profile']);
        return false;
      }
    })
    return false;
  };
}
