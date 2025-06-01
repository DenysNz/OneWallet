import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { RouteNames } from 'src/app/app-routing.module';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate {
  routeNames = RouteNames;

  constructor(private authService: AuthenticationService,
    private router: Router) { }

  canActivate() {
    if (this.authService.isUserAuthenticated()) {
      return true;
    }

    this.router.navigate([this.routeNames.LOGIN_PAGE]);
    return false;
  }
}
