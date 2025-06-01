import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { RouteNames } from 'src/app/app-routing.module';
import { ApiService } from '../services/api.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class WelcomeGuard implements CanActivate {
  routeNames = RouteNames;

  constructor(private router: Router, private apiService: ApiService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | boolean {
    if (localStorage.getItem('welcome')) {
      return true;
    } else {
      return this.apiService.getWelcomeVisited().pipe(
        map((value) => {
          if (value) {
            localStorage.setItem('welcome', 'visited');
            return true;
          }

          this.router.navigate([this.routeNames.WELCOME_PAGE]);
          return false;
        })
      );
    }
  }
}
