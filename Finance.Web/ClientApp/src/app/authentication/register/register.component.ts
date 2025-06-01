import { Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgEventBus } from 'ng-event-bus';
import { Subscription } from 'rxjs';
import { AppSettings } from 'src/app-settings';
import { RouteNames } from 'src/app/app-routing.module';
import { RegisterFrom } from 'src/app/core/forms/register-form';
import { UserRegisterDetails } from 'src/app/core/models/user-register-details.model';
import { ApiService } from 'src/app/core/services/api.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { PasswordConfirmationValidator } from 'src/app/core/validators/password-confirm-validator';
import { VerificationEmailModalComponent } from 'src/app/shared/modals/verification-email-modal/verification-email-modal.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
  registerForm!: FormGroup;
  user!: UserRegisterDetails;
  routeNames = RouteNames;
  errorList: string[] = [];
  subscriptions: Subscription[] = [];

  constructor(private PassConfValidator: PasswordConfirmationValidator,
    private apiService: ApiService,
    private modalService: NgbModal,
    private eventBus: NgEventBus,
    private router: Router,
    private authService: AuthenticationService,
    private ngZone: NgZone) {
    this.registerForm = RegisterFrom.getFormGroup();
    this.registerForm.get('confirm')?.setValidators([Validators.required,
    this.PassConfValidator.validateConfirmPassword(<AbstractControl>this.registerForm.get('password'))]);
   }

  ngOnInit(): void {
  }

  registerUser() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.user = new UserRegisterDetails(this.registerForm.value);
    this.subscriptions.push(this.apiService.registerUser(this.user)
    .subscribe(
      (res)=>{
        const modalRef = this.modalService.open(VerificationEmailModalComponent, {backdrop: 'static', centered: true});
        modalRef.componentInstance.userEmail = this.user.email;
        this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
      },
      (err)=>{
        this.errorList = err.error;
        this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
      }));
  }

  socialNetworksLoginSucceded(token: string) {
    this.authService.loginUser(token);
    this.eventBus.cast(AppSettings.EVENT_LOGGED_IN);
    this.ngZone.run(() => this.router.navigate([this.routeNames.ACCOUNTS_PAGE]));
  }

  socialNetworksLoginFailed(error: any) {
    console.log(error);
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
