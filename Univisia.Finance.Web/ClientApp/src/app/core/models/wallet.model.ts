import { CurrencyModel } from './currency.model';
import { WalletAccountModel } from './wallet-account.model';

export class WalletModel {
  accounts: WalletAccountModel[];
  currency: CurrencyModel;
  totalAmount: number;

  constructor(
    accounts: WalletAccountModel[],
    currency: CurrencyModel,
    totalAmount: number
  ) {
    this.accounts = accounts;
    this.currency = currency;
    this.totalAmount = totalAmount;
  }
}
