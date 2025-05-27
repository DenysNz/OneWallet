import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { MicrosoftShemaUrl } from '../constants/auth.constants';
import { UserRoleEnum } from '../enums/user-role.enum';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(
    private _jwtHandler: JwtHelperService
  ) { }

  public isUserAuthenticated() : boolean {
    const token = localStorage.getItem("token");
    return token!=undefined && token!='' && !this._jwtHandler.isTokenExpired(token);
  }

  public isAdmin() : boolean {
    const token = localStorage.getItem("token");
    const decodedToken = this._jwtHandler.decodeToken(token??'');
    const role = decodedToken[MicrosoftShemaUrl];

    return role == UserRoleEnum.Admin;
  }

  public loginUser(token: string) {
    localStorage.setItem("token", token);   
  }

  public logout() {
    localStorage.removeItem("token");
  }
}
