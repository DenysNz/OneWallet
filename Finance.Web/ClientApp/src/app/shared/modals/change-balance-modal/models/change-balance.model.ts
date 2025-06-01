export class ChangeBalanceModel {
    accountId: number;
    notes: string;
    amount: number;
  
    constructor(
      accountId: number,
      notes: string,
      amount: number,
    ) {
      this.accountId = accountId;
      this.notes = notes,
      this.amount = amount;
    }
  }