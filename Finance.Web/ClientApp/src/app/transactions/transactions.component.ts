import { Component, OnDestroy, OnInit } from '@angular/core';
import { TransactionsService } from 'src/app/core/services/transactions.service';
import { TransactionsModel } from 'src/app/core/models/transactions.model';
import { AccountService } from 'src/app/core/services/account.service';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { LookupModel } from '../core/models/look-up.model';
import { LoansService } from '../core/services/loans.service';
import { LoanModel } from '../core/models/loan.model';
import { ToastService } from '../core/services/toasts.service';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.scss'],
})
export class TransactionsComponent implements OnInit, OnDestroy {
  accounts: LookupModel[] = [];
  loans: LoanModel[] = [];
  transactions: TransactionsModel[] = [];
  subscriptions: any[] = [];
  accountName: string = '';
  accountTransactions: TransactionsModel[] = [];
  isAccountSelected: boolean = false;
  dropDownAccountName: string = '';
  dropDownLoanName: string = '';
  page: number = 1;
  collectionSize: number = 0;
  itemsPerPage: number = 5;
  accountId: number | null = null;
  loanId: number | null = null;
  maxPage: number = 0;
  totalCount: number = 0;
  dropDownAccountChangeString: string = 'Change account';
  dropDownLoanChangeString: string = 'Change loan';
  serverErrorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private transactionsService: TransactionsService,
    private accountsService: AccountService,
    private loansService: LoansService,
    private eventBus: NgEventBus,
    private toastsService: ToastService,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.subscriptions.push(
      this.translate.stream('TOAST').subscribe((value) => {
        this.serverErrorMessage = value.ERROR;
      })
    );
    this.accountId = parseInt(this.route.snapshot.paramMap.get('accountId')!);
    this.loanId = parseInt(this.route.snapshot.paramMap.get('loanId')!);
    this.accountId
      ? (this.accountId = this.accountId)
      : (this.accountId = null);
    this.loanId ? (this.loanId = this.loanId) : (this.loanId = null);
    this.getTotalCount();
    this.subscriptions.push(
      this.accountsService.getAccountsLookup().subscribe(
        (data) => {
          this.accounts = [...data];
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          this.loadTransactions();
        },
        () => {
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      )
    );
    this.subscriptions.push(
      this.loansService.getLoans().subscribe(
        (data) => {
          this.loans = [...data];
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          this.loadTransactions();
        },
        () => {
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      )
    );
    this.subscriptions.push(
      this.translate.stream('TRANSACTIONS').subscribe((value) => {
        this.dropDownAccountChangeString = value.DROPDOWNACCOUNT;
        this.dropDownLoanChangeString = value.DROPDOWNLOAN;
      })
    );
  }

  onChangePage(value: boolean) {
    if (this.page < this.maxPage) {
      if (this.page == 1) {
        value ? this.page++ : (this.page = this.page);
      } else {
        value ? this.page++ : this.page--;
      }
    } else {
      value ? (this.page = this.page) : this.page--;
    }
    this.loadTransactions();
  }

  onChangeAccount(account: LookupModel | null, loan: LoanModel | null) {
    this.page = 1;
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    if (account !== null) {
      this.accountId = account.id;
      this.loanId = null;
      this.loadTransactions();
    } else if (loan !== null) {
      this.loanId = loan.loanId;
      this.accountId = null;
      this.loadTransactions();
    } else {
      this.loanId = null;
      this.accountId = null;
      this.loadTransactions();
    }
    this.getTotalCount();
  }

  setAccontAndLoanName(accountId: number | null, loanId: number | null) {
    let foundAccount = this.accounts.find((e) => e.id === accountId);
    if (foundAccount?.title) {
      this.dropDownAccountName = foundAccount.title;
    } else {
      this.dropDownAccountName = this.dropDownAccountChangeString;
    }
    let foundLoan = this.loans.find((e) => e.loanId === loanId);
    if (foundLoan?.person) {
      this.dropDownLoanName =
        foundLoan.person +
        ' (' +
        foundLoan.currency.currencyName +
        ' ' +
        foundLoan.amount +
        ')';
    } else {
      this.dropDownLoanName = this.dropDownLoanChangeString;
    }
  }

  loadTransactions() {
    this.setAccontAndLoanName(this.accountId, this.loanId);
    this.subscriptions.push(
      this.transactionsService
        .getTransactions(
          this.accountId,
          this.loanId,
          this.page,
          this.itemsPerPage
        )
        .subscribe(
          (data) => {
            this.transactions = data;
            this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          },
          () => {
            this.toastsService.showDanger(this.serverErrorMessage, 5000);
            this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          }
        )
    );
  }

  getTotalCount() {
    this.subscriptions.push(
      this.transactionsService
        .getTotalCount(this.accountId, this.loanId)
        .subscribe(
          (data) => {
            this.collectionSize = data;
            this.maxPage = Math.ceil(this.collectionSize / this.itemsPerPage);
            this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          },
          () => {
            this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
            this.toastsService.showDanger(this.serverErrorMessage, 5000);
          }
        )
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((s) => {
      if (s) {
        s.unsubscribe();
      }
    });
    this.toastsService.clear();
  }
}
