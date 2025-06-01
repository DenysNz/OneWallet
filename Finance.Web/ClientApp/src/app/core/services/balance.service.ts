import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountModel } from '../models/account.model';
import { CurrencyModel } from '../models/currency.model';
import { Observable, of } from 'rxjs';
import { WalletModel } from '../models/wallet.model';
import { BalanceModel } from '../models/balance.model';
import { map } from 'rxjs/operators';
import { WalletAccountModel } from '../models/wallet-account.model';

@Injectable({
  providedIn: 'root',
})
export class BalanceService {
  constructor(private http: HttpClient) {}

  getWallets(): Observable<BalanceModel> {
    return this.http.get('account/wallet').pipe(
      map((balance: any) => {
        return new BalanceModel(
          balance.balance,
          balance.wallets.map((wallet: any) => {
            return new WalletModel(
              wallet.accounts.map((account: any) => {
                return new WalletAccountModel(
                  account.accountId,
                  account.accountName,
                  account.amount
                );
              }),
              new CurrencyModel(wallet.currency.title, wallet.currency.id),
              wallet.total
            );
          })
        );
      })
    );
  }
}
