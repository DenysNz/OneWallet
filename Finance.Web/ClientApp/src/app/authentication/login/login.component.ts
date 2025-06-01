import { Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ApiService } from 'src/app/core/services/api.service';
import { LoginForm } from 'src/app/core/forms/login-form';
import { Subscription } from 'rxjs';
import { UserLoginDetails } from 'src/app/core/models/user-login-details.model';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { Router } from '@angular/router';
import { RouteNames } from 'src/app/app-routing.module';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ResendVerificationEmailModalComponent } from 'src/app/shared/modals/resend-verification-email-modal/resend-verification-email-modal.component';
import { ResetPasswordModalComponent } from 'src/app/shared/modals/reset-password-modal/reset-password-modal.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  pageName: string = "Login";
  loginForm!: FormGroup;
  user!: UserLoginDetails;
  showResendLink!: boolean;
  error: string = "";
  routeNames = RouteNames;
  subscriptions: Subscription [] = [];
  

  constructor(private apiService: ApiService,
    private authService: AuthenticationService,
    private eventBus: NgEventBus,
    private router: Router,
    private modalService: NgbModal,
    private ngZone: NgZone) {
    this.loginForm = LoginForm.getFormGroup();
  }

  ngOnInit(): void {
  }

  loginUser() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.showResendLink = false;
    this.user = new UserLoginDetails(this.loginForm.value);
    this.subscriptions.push(this.apiService.loginUser(this.user)
      .subscribe(
        (res) => {
          this.authService.loginUser(res.token);
          this.eventBus.cast(AppSettings.EVENT_LOGGED_IN);
          this.router.navigate([this.routeNames.ACCOUNTS_PAGE]);
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
        }, 
        (err) =>{
          this.error = err.error;
          if (this.error === "Email is not verified") {
            this.showResendLink = true;
          }
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
        }));
  }

  resendVerificationEmail() {
    this.modalService.open(ResendVerificationEmailModalComponent, {centered: true});
  }

  resetPassword() {
    this.modalService.open(ResetPasswordModalComponent, {centered: true});
  }

  registerAccount(){
    this.router.navigate([this.routeNames.REGISTER_PAGE]);
  }

  socialNetworksLoginSucceded(token: string) {
    this.authService.loginUser(token);
    this.eventBus.cast(AppSettings.EVENT_LOGGED_IN);
    this.ngZone.run(() => this.router.navigate([this.routeNames.ACCOUNTS_PAGE]));
  }

  socialNetworskLoginFailed(error: any) {
    this.error = error;
    this.ngZone.run(() => this.router.navigate([this.routeNames.LOGIN_PAGE]));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => {
      if(s){
        s.unsubscribe();
      }
    });
  }
}
