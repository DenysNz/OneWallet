import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { AddCurrenciesModel } from 'src/app/setting/services/add-client-currencies.model';
import { CurrencyModel } from '../models/currency.model';

@Injectable({
  providedIn: 'root',
})
export class CurrencyService {
  
  constructor(private http: HttpClient) {}

  loadCurrencies(showIsPopular: boolean): Observable<CurrencyModel[]> {
    return this.http.get('currency/lookup?justPopular=' + showIsPopular).pipe(
      map((currencies: any) => {
        return currencies.map((currency: any) => {
          return new CurrencyModel(currency.title, currency.id);
        });
      })
    );
  }

  loadClientCurrencies(): Observable<CurrencyModel[]> {
    return this.http.get('currency/usercurrencies').pipe(
      map((currencies: any) => {
        return currencies.map((currency: any) => {
          return new CurrencyModel(currency.title, currency.id);
        });
      })
    );
  }

  updateClientCurrencies(currencies: AddCurrenciesModel[]) {
    return this.http.patch('currency/saveusercurrencies', JSON.stringify(currencies), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }
}
