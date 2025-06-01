import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { ResendEmailForm } from 'src/app/core/forms/resend-email.form';
import { ApiService } from 'src/app/core/services/api.service';
import { VerificationEmailModalComponent } from '../verification-email-modal/verification-email-modal.component';

@Component({
  selector: 'app-resend-verify-email-modal',
  templateUrl: './resend-verification-email-modal.component.html',
  styleUrls: ['./resend-verification-email-modal.component.scss']
})
export class ResendVerificationEmailModalComponent implements OnInit, OnDestroy {
  resendEmailForm!: FormGroup;
  incorectEmail: boolean = false;
  incorectEmailAddress: string = "";
  subscriptions: Subscription [] = [];

  constructor(private apiService: ApiService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal) {
    this.resendEmailForm = ResendEmailForm.GetFormGroup();
  }

  ngOnInit(): void {
  }

  resendVerifyEmail() {
    const email = this.resendEmailForm.get("email")?.value;
    this.subscriptions.push(this.apiService.sendSecurityCode(email)
    .subscribe((res) => {
      const modalRef = this.modalService.open(VerificationEmailModalComponent, {backdrop: 'static', centered: true});
      modalRef.componentInstance.userEmail = email;
      this.activeModal.close()
    },
    (err) => {
      this.incorectEmail = true;
      this.incorectEmailAddress = email;
    }));
  }

  clearErrorMessage() {
    this.incorectEmail = false;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => {
      if(s) {
        s.unsubscribe();
      }
    });
  }
}
