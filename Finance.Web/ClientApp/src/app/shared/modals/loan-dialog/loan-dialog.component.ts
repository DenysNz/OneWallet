import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbToast } from '@ng-bootstrap/ng-bootstrap';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { CurrencyModel } from 'src/app/core/models/currency.model';
import { LoanModel } from 'src/app/core/models/loan.model';
import { CurrencyService } from 'src/app/core/services/currency.service';
import { LoansService } from 'src/app/core/services/loans.service';
import { ToastService } from 'src/app/core/services/toasts.service';
import { AddLoanModel } from './models/add-loan.model';
import { formatDate } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-loan-dialog',
  templateUrl: './loan-dialog.component.html',
  styleUrls: ['./loan-dialog.component.scss'],
})
export class LoanDialogComponent implements OnInit {
  @Input() loan?: LoanModel;

  loanForm: FormGroup;
  currencies: CurrencyModel[] = [];
  subscriptions: any[] = [];
  serverErrorMessage: string = '';
  loanUpdatedMessage: string = '';
  loanUpdatedErrorMessage: string = '';
  loanAddedMessage: string = '';
  loanAddedErrorMessage: string = '';

  constructor(
    private activeModal: NgbActiveModal,
    private loansService: LoansService,
    private currencyService: CurrencyService,
    private eventBus: NgEventBus,
    private toastsService: ToastService,
    public translate: TranslateService
  ) {
    this.loanForm = new FormGroup({
      email: new FormControl('', [Validators.email]),
      deadline: new FormControl('', [Validators.maxLength(10)]),
      currency: new FormControl('', [Validators.required]),
      amount: new FormControl('', [Validators.required]),
      note: new FormControl('', [
        Validators.required,
        Validators.maxLength(15),
      ]),
      person: new FormControl('', [
        Validators.required,
        Validators.maxLength(15),
      ]),
      status: new FormControl(false),
    });
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.translate.stream('TOAST').subscribe((value) => {
        this.serverErrorMessage = value.ERROR;
        this.loanUpdatedMessage = value.LOANUPDATED;
        this.loanUpdatedErrorMessage = value.LOANUPDATEDERROR;
        this.loanAddedMessage = value.LOANADDED;
        this.loanAddedErrorMessage = value.LOANADDEDERROR;
      })
    );
    this.subscriptions.push(
      this.currencyService.loadClientCurrencies().subscribe(
        (data) => {
          this.currencies = [...data];
        },
        () => {
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
          this.activeModal.close();
        }
      )
    );
    if (this.loan) {
      this.setValues();
    }
  }

  setValues() {
    let deadline: any = this.loan?.deadline;
    deadline = formatDate(deadline, 'yyyy-dd-MM', 'en');
    this.loanForm.controls.email.setValue(this.loan?.contactEmail);
    if(this.loan?.deadline != null) this.loanForm.controls.deadline.setValue(deadline);
    this.loanForm.controls.currency.setValue(this.loan?.currency.currencyId);
    this.loanForm.controls.amount.setValue(this.loan?.amount);
    this.loanForm.controls.note.setValue(this.loan?.note);
    this.loanForm.controls.person.setValue(this.loan?.person);
  }

  close() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    let value = this.loanForm.value;
    let addLoanObject = new AddLoanModel(
      0,
      value.email,
      value.deadline.length == 0 ? null : value.deadline,
      value.amount,
      value.person,
      value.note,
      value.currency
    );
    if (this.loan) {
      addLoanObject.loanId = this.loan.loanId;
      this.subscriptions.push(
        this.loansService.updateLoan(addLoanObject).subscribe(
          () => {
            this.eventBus.cast(AppSettings.EVENT_SAVE_BUTTON_CLICKED);
            this.toastsService.showSuccess(this.loanUpdatedMessage, 5000);
            this.activeModal.close();
          },
          () => {
            this.toastsService.showDanger(this.loanUpdatedErrorMessage, 5000);
            this.activeModal.close();
          }
        )
      );
    } else {
      this.subscriptions.push(
        this.loansService.addLoan(addLoanObject).subscribe(
          () => {
            this.eventBus.cast(AppSettings.EVENT_SAVE_BUTTON_CLICKED);
            this.toastsService.showSuccess(this.loanAddedMessage, 5000);
            this.activeModal.close();
          },
          () => {
            this.toastsService.showDanger(this.loanAddedErrorMessage, 5000);
            this.activeModal.close();
          }
        )
      );
    }
  }

  dismiss() {
    this.activeModal.dismiss();
  }

  onAmountChange() {
    const amount = this.loanForm.get('amount');
    amount?.setValue(
      (this.loanForm.get('status')?.value ? 1 : -1) * Math.abs(amount.value)
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((subscription) => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }
}
