import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { AccountModel } from 'src/app/core/models/account.model';
import { CurrencyModel } from 'src/app/core/models/currency.model';
import { AccountService } from 'src/app/core/services/account.service';
import { CurrencyService } from 'src/app/core/services/currency.service';
import { ToastService } from 'src/app/core/services/toasts.service';
import { AddAccountModel } from './models/add-account.model';
import { UpdateAccountModel } from './models/update-account.model';

@Component({
  selector: 'app-account-dialog',
  templateUrl: './account-dialog.component.html',
  styleUrls: ['./account-dialog.component.scss'],
})
export class AccountDialogComponent implements OnInit {
  @Input() object?: AccountModel;

  subscriptions: any[] = [];
  accountForm: FormGroup;
  currencies: CurrencyModel[] = [];
  serverErrorMessage: string = '';
  accAddedMessage: string = '';
  accAddedErrorMessage: string = '';
  accUpdatedMessage: string = '';
  accUpdatedErrorMessage: string = '';

  constructor(
    private activeModal: NgbActiveModal,
    private currencyService: CurrencyService,
    private eventBus: NgEventBus,
    private accountService: AccountService,
    private toastsService: ToastService,
    public translate: TranslateService
  ) {
    this.accountForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(15),
      ]),
      currency: new FormControl('', [Validators.required]),
      amount: new FormControl('', [Validators.required]),
    });
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.translate.stream('TOAST').subscribe((value) => {
        this.serverErrorMessage = value.ERROR;
        this.accAddedMessage = value.ACCOUNTADDED;
        this.accAddedErrorMessage = value.ACCOUNTADDEDERROR;
        this.accUpdatedMessage = value.ACCOUNTUPDATED;
        this.accUpdatedErrorMessage = value.ACCOUNTUPDATEDERROR;
      })
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
    if (this.object) {
      this.setValues();
    }
  }

  setValues() {
    this.accountForm.controls.name.setValue(this.object?.name);
    this.accountForm.controls.currency.setValue(
      this.object?.currency.currencyId
    );
    this.accountForm.removeControl('amount');
  }

  onSave() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    let value = this.accountForm.value;
    if (this.object) {
      let addAccountObject = new UpdateAccountModel(
        0,
        value.name,
        value.currency
      );
      addAccountObject.accountId = this.object.accountId;
      this.subscriptions.push(
        this.accountService.updateAccount(addAccountObject).subscribe(
          () => {
            this.eventBus.cast(AppSettings.EVENT_SAVE_BUTTON_CLICKED);
            this.toastsService.showSuccess(this.accUpdatedMessage, 5000);
            this.activeModal.close();
          },
          () => {
            this.toastsService.showDanger(this.accUpdatedErrorMessage, 5000);
          }
        )
      );
    } else {
      let addAccountObject = new AddAccountModel(
        0,
        value.name,
        value.currency,
        value.amount
      );
      this.subscriptions.push(
        this.accountService.addAccount(addAccountObject).subscribe(
          () => {
            this.eventBus.cast(AppSettings.EVENT_SAVE_BUTTON_CLICKED);
            this.toastsService.showSuccess(this.accAddedMessage, 5000);
            this.activeModal.close();
          },
          () => {
            this.toastsService.showDanger(this.accAddedErrorMessage, 5000);
            this.activeModal.close();
          }
        )
      );
    }
  }

  onDiscard() {
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
