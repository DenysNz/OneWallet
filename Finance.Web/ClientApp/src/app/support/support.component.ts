import { Component } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { CurrencyModel } from '../core/models/currency.model';
import { SupportModel } from '../core/models/support.model';
import { SupportService } from '../core/services/support.service';
import { ToastService } from '../core/services/toasts.service';

@Component({
  selector: 'app-support',
  templateUrl: './support.component.html',
  styleUrls: ['./support.component.scss'],
})
export class SupportComponent {
  toastSuccessMessage: string = '';
  toastErrorMessage: string = '';
  token: string | undefined;
  subscriptions: any[] = [];
  suportForm: FormGroup;
  currencies: CurrencyModel[] = [];
  accountId: string | null = '';
  showForm: boolean = true;
  placeholder: string = "";

  constructor(
    private toastsService: ToastService,
    public translate: TranslateService,
    private supportService: SupportService,
  ) {
    this.suportForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      email: new FormControl('', [
        Validators.required,
        Validators.email
      ]),
      text: new FormControl('', [Validators.required]),
    });
    this.token = undefined;
  }

  ngOnInit(): void { 
    this.subscriptions.push(
      this.translate
        .stream('TOAST')
        .subscribe((value) => {
          this.toastSuccessMessage = value.REQUESTSUCCESSFULLY;
          this.toastErrorMessage = value.ERROR;
        }),
      this.translate
        .stream('SUPPORT.FORM.PLACEHOLDER')
        .subscribe((value) => {
          this.placeholder = value;
        })
    );
  }

  onSave() {
    this.showForm = false;
    let value = this.suportForm.value;
    let addSupportObject = new SupportModel(
      value.name,
      value.email,
      value.text,
      this.token
    );
    this.subscriptions.push(
      this.supportService.addSupport(addSupportObject).subscribe(
        () => {
          this.toastsService.showSuccess(this.toastSuccessMessage, 5000);
        },
        () => {
          this.toastsService.showDanger(this.toastErrorMessage, 5000);
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
