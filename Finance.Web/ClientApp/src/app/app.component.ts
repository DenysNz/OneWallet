import {
  Component,
  OnDestroy,
  OnInit,
  ChangeDetectorRef,
  AfterContentChecked,
} from '@angular/core';
import { NgEventBus } from 'ng-event-bus';
import { Subscription } from 'rxjs';
import { AppSettings } from 'src/app-settings';
import {
  NgcCookieConsentService,
  NgcNoCookieLawEvent,
  NgcStatusChangeEvent,
} from 'ngx-cookieconsent';
import { TranslateService } from '@ngx-translate/core';
// import { SwPush } from '@angular/service-worker';
// import { NewsletterService } from './core/services/newsletter.service';
// import vapidKey from '../assets/json/vapid-key.json';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit, OnDestroy, AfterContentChecked {
  private popupOpenSubscription!: Subscription;
  private popupCloseSubscription!: Subscription;
  private initializingSubscription!: Subscription;
  private initializedSubscription!: Subscription;
  private initializationErrorSubscription!: Subscription;
  private statusChangeSubscription!: Subscription;
  private revokeChoiceSubscription!: Subscription;
  private noCookieLawSubscription!: Subscription;

  showSpinner: boolean = false;
  subscriptions: any[] = [];
  isVisible: boolean = true;

  constructor(
    private eventBus: NgEventBus,
    private cdRef: ChangeDetectorRef,
    private ccService: NgcCookieConsentService,
    public translate: TranslateService,
    // private swPush: SwPush,
    // private newsletterService: NewsletterService
  ) {}

  ngOnInit(): void {
    // this.subscribeToNotifications();
    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_SPINNER_SHOW).subscribe(() => {
        this.showSpinner = true;
      })
    );

    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_SPINNER_HIDE).subscribe(() => {
        this.showSpinner = false;
      })
    );

    // subscribe to cookieconsent observables to react to main events
    this.popupOpenSubscription = this.ccService.popupOpen$.subscribe(() => {
      // you can use this.ccService.getConfig() to do stuff...
    });

    this.popupCloseSubscription = this.ccService.popupClose$.subscribe(() => {
      // you can use this.ccService.getConfig() to do stuff...
    });

    this.initializedSubscription = this.ccService.initialize$.subscribe(() => {
      // the cookieconsent has been successfully initialized.
      // It's now safe to use methods on NgcCookieConsentService that require it, like `hasAnswered()` for eg...
      console.log(`initialized: ${JSON.stringify(event)}`);
    });

    this.statusChangeSubscription = this.ccService.statusChange$.subscribe(
      (event: NgcStatusChangeEvent) => {
        // you can use this.ccService.getConfig() to do stuff...
      }
    );

    this.revokeChoiceSubscription = this.ccService.revokeChoice$.subscribe(
      () => {
        // you can use this.ccService.getConfig() to do stuff...
      }
    );

    this.noCookieLawSubscription = this.ccService.noCookieLaw$.subscribe(
      (event: NgcNoCookieLawEvent) => {
        // you can use this.ccService.getConfig() to do stuff...
      }
    );

    this.setLang();
  }

  ngAfterContentChecked(): void {
    this.cdRef.detectChanges();
  }

  setLang(): void {
    let item = localStorage.getItem('language');
    if (item) {
      let selectedLang = JSON.parse(item);
      this.translate.use(selectedLang.translateName);
    }
  }
  // Push Notification

  // subscribeToNotifications(): void {
  //   if (!this.swPush.isEnabled) {
  //     console.log('Notification is not enabled.');
  //   } else {
  //     this.subscriptions.push(
  //       this.swPush
  //         .requestSubscription({
  //           serverPublicKey: vapidKey.publicKey,
  //         })
  //         .then((sub: any) => {
  //           this.subscriptions.push(
  //             this.newsletterService.addPushSubscriber(sub).subscribe(
  //               () => {
  //                 console.log('success (stub)');
  //               },
  //               (error) => {
  //                 console.error(error);
  //               }
  //             )
  //           );
  //         })
  //         .catch((err: any) => console.error(err))
  //     );
  //   }
  // }

  ngOnDestroy(): void {
    this.subscriptions.forEach((s) => {
      if (s) {
        s.unsubscribe();
      }
    });
  }
}
