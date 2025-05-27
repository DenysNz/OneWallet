import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { inject } from '@angular/core/testing';
import { Observable, of, throwError } from 'rxjs';
import {catchError} from 'rxjs/operators'
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  baseUrl!: string;

  constructor(private httpClient: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
      this.baseUrl = baseUrl;
  }

  private _makeRequestUrl(endpoint: string, route: string) : string {
    let queryUrl = endpoint + route;
    
    if(!queryUrl.startsWith('http')) {
      queryUrl = location.protocol + "//" + endpoint + route;
    }
    return queryUrl;
  }

  private _handleError(error: HttpErrorResponse) {
    if(error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(`Backend returned code ${error.status} body was: ${JSON.stringify(error.error)}`);
    }
    return throwError(error);
  }

  private _setHeaders() : HttpHeaders {
    let headers = new HttpHeaders();

    headers = headers
    .set('Accept', 'application/json')
    .set('Content-Type', 'application/json');

    return headers;
  }

  private _get(
    endpoint: string,
    path: string,
    options?: any) : Observable<any> {
    return this.httpClient
    .get<any>(this._makeRequestUrl(endpoint,path), options)
    .pipe(catchError(this._handleError));
  }

  private _post(
    endpoint: string,
    route: string,
    body: any,
    optinos?: any) : Observable<any> {
    return this.httpClient
    .post(this._makeRequestUrl(endpoint,route), body, optinos)
    .pipe(catchError(this._handleError));
  }

  private _patch(
    endpoint : string,
    route : string,
    body : any,
    optinos?: any) : Observable<any> {
    return this.httpClient
    .patch(this._makeRequestUrl(endpoint,route), body, optinos)
    .pipe(catchError(this._handleError));
  }

  private _put(
    endpoint: string, 
    route: string, 
    body: any, 
    optinos?: any) : Observable<any> {
      return this.httpClient
      .put(this._makeRequestUrl(endpoint, route), body, optinos)
      .pipe(catchError(this._handleError));
  }

  registerUser(data: any) {
    return this._post(this.baseUrl, "authentication/register", data);
  }

  loginUser(data: any) {
    return this._post(this.baseUrl, "authentication/login", data);
  }

  changePassword(email: string, currentPassword: string, newPassword: string) 
  {
    return this._get(this.baseUrl,"authentication/changepassword/" + email + "/" + currentPassword + "/" + newPassword);
  }

  sendSecurityCode(email: string) {
    return this._get(this.baseUrl,"authentication/sendsecuritycode/" + email);
  }

  checkSecurityCode(email: string, code: string) {
    return this._get(this.baseUrl,"authentication/verifysecuritycode/" + email + "/" + code);
  }

  resetPassword(email: string, newPassword: string) {
    return this._get(this.baseUrl,"authentication/resetpassword/" + email + "/" + newPassword);
  }

  getUserDetails() {
    return this._get(this.baseUrl, "userdetails/get");
  }

  updateUserDetails(data: any) {
    return this._put(this.baseUrl, "userdetails/update", data)
  }

  getGoogleSettings() {
    return this._get(this.baseUrl, "social/googlesettings");    
  }

  authenticateViaGoogle(token: string) {
    let data = {
      credential: token 
    };
    return this._post(this.baseUrl, "social/googlelogin", data);
  }

  authenticateViaSocialNetworks(data: any) {
    return this._post(this.baseUrl, "social/socialnetworklogin", data);
  }

  getWelcomeVisited(): Observable<boolean> {
    return this.httpClient.get('userdetails/statuswelcome').pipe(
      map((boolenStatus: any) => {
        return boolenStatus.welcomePageIsVisited
      })
    ); 
  }

  saveWelcomeVisited() {
    return this.httpClient.patch('userdetails/updatestatuswelcome','', {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }
}
