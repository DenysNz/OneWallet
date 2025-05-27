export class ReasomObjectModel {
    loanId: number;
    quotes: string | null;
  
    constructor(
      loanId: number,
      quotes: string | null,
    ) {
      this.loanId = loanId;
      this.quotes = quotes;
    }
  }