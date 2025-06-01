import { WalletModel } from './wallet.model';

export class BalanceModel {
  balance: number;
  wallets: WalletModel[];

  constructor(balance: number, wallets: WalletModel[]) {
    this.balance = balance;
    this.wallets = wallets;
  }
}
