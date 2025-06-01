import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgEventBus } from 'ng-event-bus';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AddLoanTransactionModel } from 'src/app/shared/modals/add-loan-transaction/models/add-loan-transaction.model';
import { AddLoanModel } from 'src/app/shared/modals/loan-dialog/models/add-loan.model';
import { ReasomObjectModel } from 'src/app/shared/modals/reject-loan-dialod/models/reason-object.model';
import { CurrencyModel } from '../models/currency.model';
import { LoanModel } from '../models/loan.model';

@Injectable({
  providedIn: 'root',
})
export class LoansService {
  constructor(private http: HttpClient) {}

  getLoans(): Observable<LoanModel[]> {
    return this.http.get('loan/loans').pipe(
      map((loans: any) => {
        return loans.map((loan: any) => {
          return new LoanModel(
            loan.loanId,
            loan.email,
            loan.deadline,
            loan.amount,
            new CurrencyModel(loan.currencyName, loan.currencyId),
            loan.person,
            loan.note,
            loan.loanStatusId,
            loan.isOwner
          );
        });
      })
    );
  }

  addLoan(loan: AddLoanModel) {
    return this.http.patch('loan/add', JSON.stringify(loan), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }

  deleteLoan(id: number) {
    return this.http.delete('loan/delete', {
      body: JSON.stringify({ loanId: id }),
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }

  updateLoan(loan: AddLoanModel) {
    return this.http.patch('loan/update', JSON.stringify(loan), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }

  addLoanTransaction(loan: AddLoanTransactionModel) {
    return this.http.patch('loan/operation', JSON.stringify(loan), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }

  getRequestCount(): Observable<any> {
    return this.http.get('loan/getrequestcount')
  }

  approveLoanRequest( id: number){
    return this.http.patch('loan/approveloan', JSON.stringify({ loanId: id}), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }

  rejectLoanRequest( reasonObject: ReasomObjectModel ){
    return this.http.patch('loan/rejectloan', JSON.stringify(reasonObject), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }
}
