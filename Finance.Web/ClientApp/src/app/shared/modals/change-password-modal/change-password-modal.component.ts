import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgEventBus } from 'ng-event-bus';
import { Subscription } from 'rxjs';
import { AppSettings } from 'src/app-settings';
import { ChangePasswordForm } from 'src/app/core/forms/change-password.form';
import { ApiService } from 'src/app/core/services/api.service';
import { PasswordConfirmationValidator } from 'src/app/core/validators/password-confirm-validator';

@Component({
  selector: 'app-change-password-modal',
  templateUrl: './change-password-modal.component.html',
  styleUrls: ['./change-password-modal.component.scss']
})
export class ChangePasswordModalComponent implements OnInit {
  changePasswordForm!: FormGroup;
  userEmail!: string;
  isOldPasswordMatch: Boolean = true;
  subsriptions: Subscription [] = [];

  constructor(private apiService: ApiService,
    private PassConfValidator: PasswordConfirmationValidator,
    public activeModal: NgbActiveModal,
    private eventBus: NgEventBus) {
    this.changePasswordForm = ChangePasswordForm.GetFormGroup();
    this.changePasswordForm.get('confirm')?.setValidators([Validators.required,
      this.PassConfValidator.validateConfirmPassword(<AbstractControl>this.changePasswordForm.get('newPassword'))]);
  }

  ngOnInit(): void {
  }

  changePassword() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    const oldPassword = this.changePasswordForm.get('oldPassword')?.value;
    const newPassword = this.changePasswordForm.get('newPassword')?.value;
    this.subsriptions.push(this.apiService.changePassword(this.userEmail, oldPassword, newPassword)
    .subscribe((res) => {
      this.activeModal.close();
      this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
    },
    (err) => {
      this.isOldPasswordMatch = false;
      this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
    }));
  }

  clearInputOldPassword()
  {
    this.isOldPasswordMatch = true;
  }
}
