import { CurrencyModel } from "./currency.model";

export class TransactionsModel {
    date: string;
    amount: number;
    currency: CurrencyModel;
    notes: string;

    constructor(date: string, amount: number, currency: CurrencyModel, notes: string) {
        this.date = date;
        this.amount = amount;
        this.currency = currency;
        this.notes = notes;
    }
}