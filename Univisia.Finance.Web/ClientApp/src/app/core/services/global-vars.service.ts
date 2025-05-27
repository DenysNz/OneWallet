import { Injectable } from '@angular/core';
import { RouteNames } from 'src/app/app-routing.module';

@Injectable({
  providedIn: 'root'
})
export class GlobalVarsService {
  showSpinner: boolean = false;
  routeNames = RouteNames;

  constructor() { }
}
