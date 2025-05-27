import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  Input,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { CurrencyModel } from 'src/app/core/models/currency.model';

@Component({
  selector: 'app-currency-dropdown',
  templateUrl: './currency-dropdown.component.html',
  styleUrls: ['./currency-dropdown.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CurrencyDropdownComponent),
      multi: true,
    },
  ],
})
export class CurrencyDropdownComponent
  implements OnInit, ControlValueAccessor, OnDestroy
{
  @Input() currencies!: CurrencyModel[];
  @Input() previousValue?: CurrencyModel;

  subscription: Subscription | undefined;
  value: number | undefined;
  onChange!: (value: any) => void;
  onTouched!: () => void;
  disabled: boolean = false;
  dropdownName: string = 'Currency';
  choosedCurrency: CurrencyModel | undefined;

  constructor(public translate: TranslateService) {}

  writeValue(value: number): void {
    this.value = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  ngOnInit(): void {
    if (this.previousValue) {
      this.choosedCurrency = this.previousValue;
      this.dropdownName = this.previousValue.currencyName;
    }
    this.subscription = this.translate
      .stream('SHARED-COMPONENTS.CURRENCY-DROPDOWN.LABEL')
      .subscribe((value) => {
        this.dropdownName = value;
      });
  }

  onCurrencyClick(currency: CurrencyModel) {
    this.dropdownName = currency.currencyName;
    this.choosedCurrency = currency;
    this.value = this.choosedCurrency.currencyId;
    this.onChange(this.value);
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
