import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { StyleGuideComponent } from './style-guide/style-guide.component';
import { JwtModule } from '@auth0/angular-jwt';
import { NgEventBus } from 'ng-event-bus';
import { ChangePasswordModalComponent } from './shared/modals/change-password-modal/change-password-modal.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {
  NgcCookieConsentModule,
  NgcCookieConsentConfig,
} from 'ngx-cookieconsent';
import { VerificationEmailModalComponent } from './shared/modals/verification-email-modal/verification-email-modal.component';
import { ResendVerificationEmailModalComponent } from './shared/modals/resend-verification-email-modal/resend-verification-email-modal.component';
import { ResetPasswordModalComponent } from './shared/modals/reset-password-modal/reset-password-modal.component';
import { GoogleButtonComponent } from './shared/components/google-button/google-button.component';
import { FacebookButtonComponent } from './shared/components/facebook-button/facebook-button.component';
import { MsalModule, MsalService, MSAL_INSTANCE } from '@azure/msal-angular';
import { MicrosoftButtonComponent } from './shared/components/microsoft-button/microsoft-button.component';
import {
  IPublicClientApplication,
  PublicClientApplication,
} from '@azure/msal-browser';
import { WalletDoughnutComponent } from './shared/components/wallet-doughnut/wallet-doughnut.component';
import { FooterComponent } from './footer/footer.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { AccountsComponent } from './accounts/accounts.component';
import { BalanceComponent } from './balance/balance.component';
import { TransactionsComponent } from './transactions/transactions.component';
import { LoansComponent } from './loans/loans.component';
import { DeleteDialogComponent } from './shared/modals/delete-dialog/delete-dialog.component';
import { LoanDialogComponent } from './shared/modals/loan-dialog/loan-dialog.component';
import { AccountDialogComponent } from './shared/modals/account-dialog/account-dialog.component';
import { ToastComponent } from './toast/toast.component';
import { CurrencyDropdownComponent } from './shared/components/currency-dropdown/currency-dropdown.component';
import { ChangeBalanceModalComponent } from './shared/modals/change-balance-modal/change-balance-modal.component';
import { AddLoanTransactionComponent } from './shared/modals/add-loan-transaction/add-loan-transaction.component';
import { TermsComponent } from './terms/terms.component';
import { PrivacyComponent } from './privacy/privacy.component';
import { SplashScreenComponent } from './splash-screen/splash-screen.component';
import { SettingComponent } from './setting/setting.component';
import { LanguageDropdownComponent } from './shared/components/language-dropdown/language-dropdown.component';
import { CheckboxComponent } from './shared/components/checkbox/checkbox.component';
import {
  RECAPTCHA_SETTINGS,
  RecaptchaFormsModule,
  RecaptchaModule,
  RecaptchaSettings,
} from 'ng-recaptcha';
import { SupportComponent } from './support/support.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { UserSettingsComponent } from './shared/components/user-settings/user-settings.component';
import { WelcomePageComponent } from './welcome-page/welcome-page.component';
import { RejectLoanDialodComponent } from './shared/modals/reject-loan-dialod/reject-loan-dialod.component';
import { DropdownComponent } from './shared/components/dropdown/dropdown.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}

const cookieConfig: NgcCookieConsentConfig = {
  cookie: {
    domain: '1wallet.pro',
  },

  position: 'bottom-right',
  theme: 'classic',
  palette: {
    popup: {
      background: '#ffffff',
      text: '#000000',
      link: '#000000',
    },
    button: {
      background: '#774dd2',
      text: '#ffffff',
      border: 'transparent',
    },
  },
  type: 'info',
  content: {
    message:
      'This website uses cookies to ensure you get the best experience on our website.',
    dismiss: 'Allow cookies',
    deny: 'Decline',
    link: 'Learn more',
    href: 'https://cookiesandyou.com',
    policy: 'Cookie Policy',
  },
};

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication(environment.msalConfig);
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    LoginComponent,
    RegisterComponent,
    StyleGuideComponent,
    ChangePasswordModalComponent,
    VerificationEmailModalComponent,
    ResendVerificationEmailModalComponent,
    ResetPasswordModalComponent,
    GoogleButtonComponent,
    FacebookButtonComponent,
    MicrosoftButtonComponent,
    WalletDoughnutComponent,
    FooterComponent,
    AccountsComponent,
    BalanceComponent,
    TransactionsComponent,
    LoansComponent,
    DeleteDialogComponent,
    LoanDialogComponent,
    AccountDialogComponent,
    ToastComponent,
    CurrencyDropdownComponent,
    ChangeBalanceModalComponent,
    AddLoanTransactionComponent,
    TermsComponent,
    PrivacyComponent,
    SplashScreenComponent,
    SettingComponent,
    LanguageDropdownComponent,
    CheckboxComponent,
    SupportComponent,
    UserSettingsComponent,
    WelcomePageComponent,
    RejectLoanDialodComponent,
    DropdownComponent,
  ],
  imports: [
    NgcCookieConsentModule.forRoot(cookieConfig),
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    NgbModule,
    RecaptchaModule,
    RecaptchaFormsModule,
    MsalModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
      },
    }),
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: environment.production,
      // Register the ServiceWorker as soon as the app is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000',
    }),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
      defaultLanguage: 'en',
    }),
  ],
  providers: [
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory,
    },
    {
      provide: RECAPTCHA_SETTINGS,
      useValue: {
        siteKey: environment.recaptcha.siteKey,
      } as RecaptchaSettings,
    },
    MsalService,
    NgEventBus,
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory,
    },
    MsalService,
    NgEventBus,
  ],
  bootstrap: [AppComponent],
  entryComponents: [LoanDialogComponent],
})
export class AppModule {}
