export class AddAccountModel {
  accountId: number;
  name: string;
  currencyId: number;
  amount: number;

  constructor(
    accountId: number,
    name: string,
    currencyId: number,
    amount: number
  ) {
    this.accountId = accountId;
    this.name = name;
    this.currencyId = currencyId;
    this.amount = amount;
  }
}
