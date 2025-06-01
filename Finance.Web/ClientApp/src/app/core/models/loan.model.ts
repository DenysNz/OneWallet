import { CurrencyModel } from './currency.model';

export class LoanModel {
  loanId: number;
  contactEmail: string | null;
  deadline: Date;
  amount: number;
  currency: CurrencyModel;
  person: string;
  note: string;
  loanStatusId: number;
  isOwner: boolean;

  constructor(
    loanId: number,
    contactEmail: string | null,
    deadline: Date,
    amount: number,
    currency: CurrencyModel,
    person: string,
    note: string,
    loanStatusId: number,
    isOwner: boolean,
  ) {
    this.loanId = loanId;
    this.contactEmail = contactEmail;
    this.deadline = deadline;
    this.amount = amount;
    this.currency = currency;
    this.person = person;
    this.note = note;
    this.loanStatusId = loanStatusId;
    this.isOwner = isOwner;
  }
}