export class WalletAccountModel {
  accountId: number;
  accountName: string;
  amount: number;
  constructor(accountId: number, accountName: string, amount: number) {
    this.accountId = accountId;
    this.accountName = accountName;
    this.amount = amount;
  }
}
