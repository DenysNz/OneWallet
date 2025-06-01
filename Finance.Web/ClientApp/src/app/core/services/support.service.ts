import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SupportModel } from '../models/support.model';

@Injectable({
  providedIn: 'root'
})
export class SupportService {

  constructor(private http: HttpClient) { }

  addSupport(supportObject: SupportModel) {
    return this.http.patch('support/requestsupport', JSON.stringify(supportObject), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }
}
