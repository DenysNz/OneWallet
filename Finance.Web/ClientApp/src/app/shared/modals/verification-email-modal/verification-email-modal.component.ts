import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgEventBus } from 'ng-event-bus';
import { Subscription } from 'rxjs';
import { AppSettings } from 'src/app-settings';
import { RouteNames } from 'src/app/app-routing.module';
import { ApiService } from 'src/app/core/services/api.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';

@Component({
  selector: 'app-verify-email-modal',
  templateUrl: './verification-email-modal.component.html',
  styleUrls: ['./verification-email-modal.component.scss']
})
export class VerificationEmailModalComponent implements OnInit, OnDestroy {
  @ViewChild('securityCode') inputSecurityCode: any;
  userEmail!: string;
  isSecurityCodeInvalid: boolean = false;
  isResendEmail: boolean = false;
  routeNames = RouteNames;
  subscriptions: Subscription [] = [];

  constructor(private apiService: ApiService,
    private activeModal: NgbActiveModal,
    private router: Router,
    private eventBus: NgEventBus,
    private authService: AuthenticationService) { }

  ngOnInit(): void {
  }

  checkSecurityCode(value: string) {
    this.isSecurityCodeInvalid = false;
    if(value.length == 5) {
      this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
      this.subscriptions.push(this.apiService.checkSecurityCode(this.userEmail, value.toUpperCase())
      .subscribe((res) => {
        this.authService.loginUser(res.token);
        this.eventBus.cast(AppSettings.EVENT_LOGGED_IN);
        this.router.navigate([this.routeNames.ACCOUNTS_PAGE]);
        this.activeModal.close();
        this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
      }, 
      (err) => {
        this.isSecurityCodeInvalid = true;
        this.isResendEmail = false;
        this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
      }));
    }
  }

  resendVerificationEmail() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.inputSecurityCode.nativeElement.value = '';
    this.isSecurityCodeInvalid = false;
    this.isResendEmail = true;
    this.subscriptions.push(this.apiService.sendSecurityCode(this.userEmail)
      .subscribe((res) => {
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
      if(s)
      {
        s.unsubscribe();
      }
    });
  }
}
