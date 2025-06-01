import { AccountModel } from "./account.model";

export class DoughnutModel {
    accounts: AccountModel[];
    currency: string;
    balance: number;

    constructor(accounts: AccountModel[], currency: string, balance: number) {
        this.accounts = accounts;
        this.currency = currency;
        this.balance = balance;
    }
}