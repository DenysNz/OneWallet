import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgEventBus } from 'ng-event-bus';
import { Subscription } from 'rxjs';
import { AppSettings } from 'src/app-settings';
import { RouteNames } from 'src/app/app-routing.module';
import { ResetPasswordFrom } from 'src/app/core/forms/reset-password.form';
import { ApiService } from 'src/app/core/services/api.service';
import { PasswordConfirmationValidator } from 'src/app/core/validators/password-confirm-validator';

@Component({
  selector: 'app-reset-password-modal',
  templateUrl: './reset-password-modal.component.html',
  styleUrls: ['./reset-password-modal.component.scss']
})
export class ResetPasswordModalComponent implements OnInit, OnDestroy {
  resetPasswordForm!: FormGroup;
  routeNames = RouteNames;
  showEmailModal: boolean = true;
  showSecurityCodeModal: boolean = false;
  showResetPasswordModal: boolean = false;
  emailExist: boolean = true;
  isSecurityCodeInvalid: boolean = false;
  isResendSecurityCode: boolean = false;
  incorectEmailAddress: string = "";
  email: string = "";
  subscriptions: Subscription [] = [];

  constructor(private PassConfValidator: PasswordConfirmationValidator,
    private apiService: ApiService,
    private activeModal: NgbActiveModal,
    private eventBus: NgEventBus) {
    this.resetPasswordForm = ResetPasswordFrom.GetFormGroup();
    this.resetPasswordForm.get('confirm')?.setValidators([Validators.required,
      this.PassConfValidator.validateConfirmPassword(<AbstractControl>this.resetPasswordForm.get('password'))]);
  }

  ngOnInit(): void {
  }

  sendSecurityCode(isResendSecuritycode: boolean) {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.email = this.resetPasswordForm.get("email")?.value;
    this.subscriptions.push(this.apiService.sendSecurityCode(this.email)
    .subscribe((res) => {
      if(isResendSecuritycode) {
        this.isSecurityCodeInvalid = false;
        this.isResendSecurityCode = true;
      }
      else {
        this.showEmailModal = false;
        this.showSecurityCodeModal = true;
      }
      this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
    }, 
    (err) => {
      this.emailExist = false;
      this.incorectEmailAddress = this.email;
      this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
    }));
  }
  
  checkSecurityCode(securityCode: string) {
    this.isSecurityCodeInvalid = false;
    this.isResendSecurityCode = false;
    if(securityCode.length === 5) {
      this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
      this.subscriptions.push(this.apiService.checkSecurityCode(this.email, securityCode.toUpperCase())
      .subscribe((res) => {
        this.showSecurityCodeModal = false;
        this.showResetPasswordModal = true;
        this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
      },
      (err) => {
        this.isSecurityCodeInvalid = true;
        this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
      }));
    }
  }

  resetPassword() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    const newPassword = this.resetPasswordForm.get("password")?.value;
    this.subscriptions.push(this.apiService.resetPassword(this.email, newPassword)
    .subscribe((res) => {
      this.activeModal.close();
      this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
    }, 
    (err) => {
      this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
    }));
  }

  dismiss() {
    this.activeModal.dismiss();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => {
      if(s) {
        s.unsubscribe();
      }
    });
  }
}
