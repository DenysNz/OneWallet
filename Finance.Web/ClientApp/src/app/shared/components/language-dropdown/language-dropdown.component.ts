import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  Input,
  OnInit,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { LanguageModel } from './models/language.model';

@Component({
  selector: 'app-language-dropdown',
  templateUrl: './language-dropdown.component.html',
  styleUrls: ['./language-dropdown.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => LanguageDropdownComponent),
      multi: true,
    },
  ],
})
export class LanguageDropdownComponent implements OnInit, ControlValueAccessor {
  value!: LanguageModel;
  onChange!: (value: any) => void;
  onTouched!: () => void;
  disabled: boolean = false;
  languages: LanguageModel[] = [
    new LanguageModel('en', 1, 'English', 'GBP'),
    new LanguageModel('ua', 2, 'Ukrainian', 'UAH'),
  ];

  constructor(public translate: TranslateService) {}

  ngOnInit(): void {
    let lang = localStorage.getItem('language');
    lang ? (this.value = JSON.parse(lang)) : (this.value = this.languages[0]);
  }

  writeValue(value: LanguageModel): void {
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

  onLanguageClick(language: LanguageModel) {
    localStorage.setItem('language', JSON.stringify(language));
    this.translate.use(language.translateName);
    this.value = language;
  }

  getLanguageById(id: number) {}
}
