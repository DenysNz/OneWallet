export class AddLoanTransactionModel {
    loanId: number;
    notes: string;
    amount: number;
  
    constructor(
      loanId: number,
      notes: string,
      amount: number,
    ) {
      this.loanId = loanId;
      this.notes = notes,
      this.amount = amount;
    }
  }