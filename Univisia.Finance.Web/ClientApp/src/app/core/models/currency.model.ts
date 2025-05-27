export class CurrencyModel {
    currencyName: string;
    currencyId: number;

    constructor(currencyName: string, currencyId: number) {
        this.currencyName = currencyName;
        this.currencyId = currencyId;
    }
}