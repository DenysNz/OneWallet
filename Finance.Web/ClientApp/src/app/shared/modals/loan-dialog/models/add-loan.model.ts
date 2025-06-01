export class AddLoanModel {
  loanId: number;
  contactEmail: string | null;
  deadline: Date | null;
  amount: number;
  person: string;
  note: string;
  currencyId: number;

  constructor(
    loanId: number,
    contactEmail: string | null,
    deadline: Date | null,
    amount: number,
    person: string,
    note: string,
    currencyId: number,
  ) {
    this.loanId = loanId;
    this.contactEmail = contactEmail;
    this.deadline = deadline;
    this.amount = amount;
    this.person = person;
    this.note = note;
    this.currencyId = currencyId;
  }
}