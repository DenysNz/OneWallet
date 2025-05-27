import { Injectable } from '@angular/core';
import { Observable, of } from "rxjs";
import { HttpClient, HttpParams } from '@angular/common/http';
import { TransactionsModel } from "../models/transactions.model";
import { map } from 'rxjs/operators';
import { CurrencyModel } from '../models/currency.model';
@Injectable({
  providedIn: 'root'
})

export class TransactionsService {

  constructor(private http: HttpClient) { }

  getTransactions(accountId: any, loanId: any, page: number, itemsPerPage: number): Observable<TransactionsModel[]> {
    let startIndex = 1 + itemsPerPage * (page - 1);
    let endIndex = startIndex + itemsPerPage - 1;
    let options = {
      params: new HttpParams().set('accountId', accountId).set('loanId', loanId).set('startIndex', startIndex).set('endIndex', endIndex),
    };
    let transactions = this.http.get('transaction/transactions', options).pipe(
      map((transactions: any) => {
        return transactions.map((transactions: any) => {
          return new TransactionsModel(
            transactions.date,
            transactions.amount,
            new CurrencyModel(transactions.currencyName, transactions.currencyId),
            transactions.notes,
          );
        });
      })
    );
    return transactions;
  }

getTotalCount(accountId: any, loanId: any,): Observable<number>{
  let options = {
    params: new HttpParams().set('accountId', accountId).set('loanId', loanId),
  };
  return this.http.get('transaction/transactionscount', options).pipe(
    map((totalCount: any) => {
      return totalCount;
    })
  );
}
}





