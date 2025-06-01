import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AccountModel } from '../models/account.model';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { CurrencyModel } from '../models/currency.model';
import { map } from 'rxjs/operators';
import { LookupModel } from '../models/look-up.model';
import { AddAccountModel } from 'src/app/shared/modals/account-dialog/models/add-account.model';
import { UpdateAccountModel } from 'src/app/shared/modals/account-dialog/models/update-account.model';
import { ChangeBalanceModel } from 'src/app/shared/modals/change-balance-modal/models/change-balance.model';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private http: HttpClient) {}

  getAccountsLookup(): Observable<LookupModel[]> {
    return this.http.get('account/lookup').pipe(
      map((accounts: any) => {
        return accounts.map((account: any) => {
          return new LookupModel(account.title, account.id);
        });
      })
    );
  }

  getAccounts(showDeletedAccounts: boolean): Observable<any> {
    return this.http
      .get('account/accounts?includeDeleted=' + showDeletedAccounts)
      .pipe(
        map((accounts: any) => {
          return accounts.map((account: any) => {
            return new AccountModel(
              account.name,
              account.amount,
              new CurrencyModel(account.currencyName, account.currencyId),
              account.accountId,
              account.isDeleted
            );
          });
        })
      );
  }

  deleteAccount(accountId: number): Observable<any> {
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    let body = JSON.stringify({ AccountId: accountId });
    return this.http.delete('account/delete', {
      body: body,
      headers: headers,
    });
  }

  addAccount(account: AddAccountModel) {
    return this.http.patch('account/add', JSON.stringify(account), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }

  updateAccount(account: UpdateAccountModel) {
    return this.http.patch('account/update', JSON.stringify(account), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }

  changeBalance(account: ChangeBalanceModel) {
    return this.http.patch('account/changebalance', JSON.stringify(account), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }
}

