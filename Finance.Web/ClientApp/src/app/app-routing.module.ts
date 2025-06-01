import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import { AccountsComponent } from './accounts/accounts.component';
import { BalanceComponent } from './balance/balance.component';
import { LoansComponent } from './loans/loans.component';
import { TransactionsComponent } from './transactions/transactions.component';
import { StyleGuideComponent } from './style-guide/style-guide.component';
import { TermsComponent } from './terms/terms.component';
import { PrivacyComponent } from './privacy/privacy.component';
import { AuthenticationGuard } from './core/guards/authentication.guard';
import { SettingComponent } from './setting/setting.component';
import { SupportComponent } from './support/support.component';
import { WelcomePageComponent } from './welcome-page/welcome-page.component';
import { WelcomeGuard } from './core/guards/welcome.guard';

class RouteNames {
  public static LOGIN_PAGE = "login";
  public static REGISTER_PAGE = "register";
  public static SETTING_PAGE = "setting"
  public static ACCOUNTS_PAGE = "accounts";
  public static BALANCE_PAGE = "balance";
  public static TRANSACTIONS_PAGE = "transactions";
  public static TRANSACTIONS_PAGE_FROM_ACCOUNTS = "transactions/account/:accountId";
  public static TRANSACTIONS_PAGE_FROM_LOANS = "transactions/loan/:loanId";
  public static LOANS_PAGE = "loans";
  public static STYLE_GUIDE = "style-guide";
  public static SUPPORT_PAGE = "support";
  public static TERMS = "terms";
  public static PRIVACY = "privacy";
  public static WELCOME_PAGE = "welcome";
}

const routes: Routes = [
  { path: "", component: AccountsComponent, canActivate: [AuthenticationGuard, WelcomeGuard]},
  { path: RouteNames.LOGIN_PAGE, component: LoginComponent},
  { path: RouteNames.REGISTER_PAGE, component: RegisterComponent},
  { path: RouteNames.SETTING_PAGE, component: SettingComponent, canActivate: [AuthenticationGuard, WelcomeGuard]},
  { path: RouteNames.ACCOUNTS_PAGE, component: AccountsComponent, canActivate: [AuthenticationGuard, WelcomeGuard]},
  { path: RouteNames.SUPPORT_PAGE, component: SupportComponent},
  { path: RouteNames.BALANCE_PAGE, component: BalanceComponent, canActivate: [AuthenticationGuard, WelcomeGuard]},
  { path: RouteNames.TRANSACTIONS_PAGE, component: TransactionsComponent, canActivate: [AuthenticationGuard, WelcomeGuard]},
  { path: RouteNames.TRANSACTIONS_PAGE_FROM_ACCOUNTS, component: TransactionsComponent, canActivate: [AuthenticationGuard, WelcomeGuard]},
  { path: RouteNames.TRANSACTIONS_PAGE_FROM_LOANS, component: TransactionsComponent, canActivate: [AuthenticationGuard, WelcomeGuard]},
  { path: RouteNames.LOANS_PAGE, component: LoansComponent, canActivate: [AuthenticationGuard, WelcomeGuard]},
  { path: RouteNames.STYLE_GUIDE, component: StyleGuideComponent },
  { path: RouteNames.TERMS, component: TermsComponent },
  { path: RouteNames.PRIVACY, component: PrivacyComponent },
  { path: RouteNames.WELCOME_PAGE, component: WelcomePageComponent, canActivate: [AuthenticationGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

class AppRoutingModule { }

export {AppRoutingModule, RouteNames}