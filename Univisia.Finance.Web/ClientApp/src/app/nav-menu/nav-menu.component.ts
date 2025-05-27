import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { RouteNames } from '../app-routing.module';
import { NavigationStart, Event as NavigationEvent } from '@angular/router';
import { AuthenticationService } from '../core/services/authentication.service';
import { LoansService } from '../core/services/loans.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent implements OnInit, OnDestroy {
  isLoggedin: boolean = false;
  subscriptions: any[] = [];
  count: number = 0;
  serverErrorMessage: string = '';
  showLink!: boolean;
  getrequestcounts: any[] = [];

  constructor(
    private router: Router,
    private loansService: LoansService,
    private authService: AuthenticationService,
    public eventBus: NgEventBus,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.translate.stream('TOAST').subscribe((value) => {
        this.serverErrorMessage = value.ERROR;
      })
    );

    this.isLoggedin = this.authService.isUserAuthenticated();
    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_LOGGED_IN).subscribe(() => {
        this.isLoggedin = true;
        this.getRequestCount();
      })
    );
    if(this.authService.isUserAuthenticated()){
      this.getRequestCount();
    };

    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_LOGGED_OUT).subscribe(() => {
        this.isLoggedin = false;
        this.getRequestCountsClear();
      })
    );

    this.subscriptions.push(
      this.router.events.subscribe((event: NavigationEvent) => {
        if (event instanceof NavigationStart) {
          this.showLink = event.url == '/login' || event.url == '/register';
        }
      })
    );
  }

  getRequestCount(){
    this.getrequestcounts.push(
      this.loansService.getRequestCount().subscribe((data) => {
        this.count = data;
      })
    );

    this.getrequestcounts.push(
      this.eventBus.on(AppSettings.EVENT_COUNT_RESTART).subscribe(() => {
        this.loansService.getRequestCount().subscribe((data) => {
          this.count = data;
        });
      })
    );
  }

  getRequestCountsClear() {
    this.getrequestcounts.forEach((s) => {
      if (s) {
        s.unsubscribe();
      }
    });
  }

  logout() {
    this.authService.logout();
    this.eventBus.cast(AppSettings.EVENT_LOGGED_OUT);
    this.router.navigate([RouteNames.LOGIN_PAGE]);
  }

  onLoginNavigate() {
    this.router.navigate([RouteNames.LOGIN_PAGE]);
  }

  onRegisterNavigate() {
    this.router.navigate([RouteNames.REGISTER_PAGE]);
  }

  ngOnDestroy(): void {
    this.getRequestCountsClear();
    this.subscriptions.forEach((s) => {
      if (s) {
        s.unsubscribe();
      }
    });
  }
}
