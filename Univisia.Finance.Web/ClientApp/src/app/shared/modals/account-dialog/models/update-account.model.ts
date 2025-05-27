export class UpdateAccountModel {
    accountId: number;
    name: string;
    currencyId: number;
  
    constructor(
      accountId: number,
      name: string,
      currencyId: number,
    ) {
      this.accountId = accountId;
      this.name = name;
      this.currencyId = currencyId;
    }
  }