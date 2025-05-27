import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { AccountModel } from 'src/app/core/models/account.model';
import { CurrencyModel } from 'src/app/core/models/currency.model';
import { DropdownModel } from 'src/app/core/models/dropdown.model';
import { LookupModel } from 'src/app/core/models/look-up.model';
import { AccountService } from 'src/app/core/services/account.service';
import { CurrencyService } from 'src/app/core/services/currency.service';
import { ToastService } from 'src/app/core/services/toasts.service';
import { ChangeBalanceModel } from './models/change-balance.model';

@Component({
  selector: 'app-change-balance-modal',
  templateUrl: './change-balance-modal.component.html',
  styleUrls: ['./change-balance-modal.component.scss'],
})
export class ChangeBalanceModalComponent implements OnInit {
  accounts: LookupModel[] = [];
  subscriptions: any[] = [];
  accountForm: FormGroup;
  currencies: CurrencyModel[] = [];
  accountId: string | null = '';
  serverErrorMessage: string = '';
  balanceChangedMessage: string = '';
  balanceChangedErrorMessage: string = '';
  dropdownData: DropdownModel[] = [];

  constructor(
    private activeModal: NgbActiveModal,
    private currencyService: CurrencyService,
    private eventBus: NgEventBus,
    private accountService: AccountService,
    private toastsService: ToastService,
    public translate: TranslateService
  ) {
    this.accountForm = new FormGroup({
      account: new FormControl('', [Validators.required]),
      notes: new FormControl('', [
        Validators.required,
        Validators.maxLength(15),
      ]),
      amount: new FormControl('', [Validators.required]),
      status: new FormControl(false),
    });
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.translate.stream('TOAST').subscribe((value) => {
        this.serverErrorMessage = value.ERROR;
        this.balanceChangedMessage = value.BALANCECHANGED;
        this.balanceChangedErrorMessage = value.BALANCECHANGEDERROR;
      })
    );

    this.accountId = localStorage.getItem('accountId');
    if (this.accountId) {
      this.accountForm.controls.account.setValue(this.accountId);
    }

    this.subscriptions.push(
      this.accountService.getAccountsLookup().subscribe(
        (data: LookupModel[]) => {
          this.accounts = [...data];
          this.accounts.forEach((account: LookupModel) => {
            this.dropdownData.push(
              new DropdownModel(account.title, account.id)
            );
          });
        },
        () => {
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      )
    );

    this.subscriptions.push(
      this.currencyService.loadClientCurrencies().subscribe((data) => {
        this.currencies = [...data];
      })
    );
  }

  onAmountChange(): void {
    const amount = this.accountForm.get('amount');
    amount?.setValue(
      (this.accountForm.get('status')?.value ? 1 : -1) * Math.abs(amount.value)
    );
  }

  setAccountId(accountId: number): void {
    localStorage.setItem('accountId', `${accountId}`);
  }

  onSave(): void {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    let value = this.accountForm.value;
    let addAccountObject = new ChangeBalanceModel(
      value.account,
      value.notes,
      value.amount
    );
    this.setAccountId(value.account);
    this.subscriptions.push(
      this.accountService.changeBalance(addAccountObject).subscribe(
        () => {
          this.eventBus.cast(AppSettings.EVENT_SAVE_BUTTON_CLICKED);
          this.toastsService.showSuccess(this.balanceChangedMessage, 5000);
          this.activeModal.close();
        },
        () => {
          this.toastsService.showDanger(this.balanceChangedErrorMessage, 5000);
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
