import { CurrencyModel } from './currency.model';

export class AccountModel {
  name: string;
  amount: number;
  currency: CurrencyModel;
  accountId: number;
  isDeleted: boolean;

  constructor(
    name: string,
    amount: number,
    currency: CurrencyModel,
    accountId: number,
    isDeleted: boolean
  ) {
    this.name = name;
    this.amount = amount;
    this.currency = currency;
    this.accountId = accountId;
    this.isDeleted = isDeleted;
  }
}
