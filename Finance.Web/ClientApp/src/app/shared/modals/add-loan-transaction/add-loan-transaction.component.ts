import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { CurrencyModel } from 'src/app/core/models/currency.model';
import { DropdownModel } from 'src/app/core/models/dropdown.model';
import { LoanModel } from 'src/app/core/models/loan.model';
import { CurrencyService } from 'src/app/core/services/currency.service';
import { LoansService } from 'src/app/core/services/loans.service';
import { ToastService } from 'src/app/core/services/toasts.service';
import { AddLoanTransactionModel } from './models/add-loan-transaction.model';

@Component({
  selector: 'app-add-loan-transaction',
  templateUrl: './add-loan-transaction.component.html',
  styleUrls: ['./add-loan-transaction.component.scss'],
})
export class AddLoanTransactionComponent implements OnInit {
  loans: LoanModel[] = [];
  subscriptions: any[] = [];
  loanForm: FormGroup;
  currencies: CurrencyModel[] = [];
  loanId: string | null = '';
  serverErrorMessage: string = '';
  loanTransactionAddedMessage: string = '';
  loanTransactionAddedErrorMessage: string = '';
  dropdownData: DropdownModel[] = [];

  constructor(
    private activeModal: NgbActiveModal,
    private currencyService: CurrencyService,
    private eventBus: NgEventBus,
    private loanService: LoansService,
    private toastsService: ToastService,
    public translate: TranslateService
  ) {
    this.loanForm = new FormGroup({
      loan: new FormControl('', [Validators.required]),
      notes: new FormControl('', [Validators.required]),
      amount: new FormControl('', [Validators.required]),
      status: new FormControl(false),
    });
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.translate.stream('TOAST').subscribe((value) => {
        this.serverErrorMessage = value.ERROR;
        this.loanTransactionAddedMessage = value.LOANTRANSACTION;
        this.loanTransactionAddedErrorMessage = value.LOANTRANSACTIONERROR;
      })
    );
    this.loanId = localStorage.getItem('loanId');
    if (this.loanId) {
      this.loanForm.controls.loan.setValue(this.loanId);
    }
    this.subscriptions.push(
      this.loanService.getLoans().subscribe(
        (data: LoanModel[]) => {
          this.loans = [...data];
          this.loans.forEach((loan: LoanModel) => {
            this.dropdownData.push(
              new DropdownModel(
                `${loan.person} ${loan.currency.currencyName} (${loan.amount})`,
                loan.loanId
              )
            );
          });
        },
        () => {
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      )
    );
    this.subscriptions.push(
      this.currencyService.loadClientCurrencies().subscribe(
        (data) => {
          this.currencies = [...data];
        },
        () => {
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      )
    );
  }

  setLoanId(loanId: number): void {
    localStorage.setItem('loanId', `${loanId}`);
  }

  onAmountChange(): void {
    const amount = this.loanForm.get('amount');
    amount?.setValue(
      (this.loanForm.get('status')?.value ? 1 : -1) * Math.abs(amount.value)
    );
  }

  onSave(): void {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    let value = this.loanForm.value;
    let addLoanObject = new AddLoanTransactionModel(
      value.loan,
      value.notes,
      value.amount
    );
    this.setLoanId(value.loan);
    this.subscriptions.push(
      this.loanService.addLoanTransaction(addLoanObject).subscribe(
        () => {
          this.eventBus.cast(AppSettings.EVENT_SAVE_BUTTON_CLICKED);
          this.toastsService.showSuccess(
            this.loanTransactionAddedMessage,
            5000
          );
          this.activeModal.close(addLoanObject);
        },
        () => {
          this.toastsService.showDanger(
            this.loanTransactionAddedErrorMessage,
            5000
          );
          this.activeModal.close();
        }
      )
    );
  }

  onDiscard(): void {
    this.activeModal.dismiss();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((subscription) => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }
}
