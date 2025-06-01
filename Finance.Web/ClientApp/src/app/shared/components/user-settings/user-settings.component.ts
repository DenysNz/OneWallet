import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NgEventBus } from 'ng-event-bus';
import { combineLatest } from 'rxjs';
import { AppSettings } from 'src/app-settings';
import { RouteNames } from 'src/app/app-routing.module';
import { CurrencyModel } from 'src/app/core/models/currency.model';
import { ApiService } from 'src/app/core/services/api.service';
import { CurrencyService } from 'src/app/core/services/currency.service';
import { ToastService } from 'src/app/core/services/toasts.service';
import { AddCurrenciesModel } from 'src/app/setting/services/add-client-currencies.model';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss'],
})
export class UserSettingsComponent {
  @Input() isWelcome!: boolean;
  settingForm: FormGroup;
  pageName: string = 'Settings';
  subscriptions: any[] = [];
  currencies: CurrencyModel[] = [];
  clientCurrencies: CurrencyModel[] = [];
  serverErrorMessage: string = '';
  settingsSavedMessage: string = '';
  routeNames = RouteNames;

  constructor(
    private currencyService: CurrencyService,
    private toastsService: ToastService,
    private eventBus: NgEventBus,
    public translate: TranslateService,
    private apiService: ApiService,
    private router: Router
  ) {
    this.settingForm = new FormGroup({});
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.translate.stream('SETTING-PAGE').subscribe((value) => {
        this.serverErrorMessage = value.ERROR;
        this.settingsSavedMessage = value.SUCCESSFULLY;
      })
    );
    this.loadData();
  }

  loadData() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    let observables = [this.currencyService.loadCurrencies(false)];
    if (this.isWelcome) {
      observables.push(this.currencyService.loadCurrencies(true));
    } else {
      observables.push(this.currencyService.loadClientCurrencies());
    }
    this.subscriptions.push(
      combineLatest(observables).subscribe(
        (currencies) => {
          currencies[0].forEach((currency) => {
            this.settingForm.addControl(
              currency.currencyName,
              new FormControl(false)
            );
          });
          currencies[1].forEach((currency) => {
            this.settingForm.controls[currency.currencyName].setValue(
              currency.currencyId
            );
          });
          this.clientCurrencies = [...currencies[1]];
          this.currencies = [...currencies[0]];
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
        },
        () => {
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      )
    );
  }

  onChange() {
    let formValue = this.settingForm.value;
    let currneciesArr = [];
    for (let key in formValue) {
      if (formValue[key]) {
        currneciesArr.push(new AddCurrenciesModel(formValue[key], key));
      }
    }
    this.subscriptions.push(
      this.currencyService.updateClientCurrencies(currneciesArr).subscribe(
        () => {},
        () => {
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      )
    );
  }

  onClickNext() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.onChange();
    this.subscriptions.push(
      this.apiService.saveWelcomeVisited().subscribe(
        () => {
          localStorage.setItem('welcome', 'visited');
          this.router.navigate([this.routeNames.ACCOUNTS_PAGE]);
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
        },
        () => {
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
        }
      )
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((subscription) => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
    this.toastsService.clear();
  }
}
