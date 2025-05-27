import { Component, OnInit, OnDestroy } from '@angular/core';
import { BalanceService } from 'src/app/core/services/balance.service';
import { Subscription } from 'rxjs';
import { WalletModel } from 'src/app/core/models/wallet.model';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { ToastService } from '../core/services/toasts.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-balance',
  templateUrl: './balance.component.html',
  styleUrls: ['./balance.component.scss'],
})
export class BalanceComponent implements OnInit, OnDestroy {
  wallets: WalletModel[] = [];
  subscriptions: any[] = [];
  totalAmount?: number;
  serverErrorMessage: string = '';

  constructor(
    private balanceService: BalanceService,
    private eventBus: NgEventBus,
    private toastsService: ToastService,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.subscriptions.push(
      this.translate.stream('TOAST').subscribe((value) => {
        this.serverErrorMessage = value.ERROR;
      })
    );
    this.subscriptions.push(
      this.balanceService.getWallets().subscribe(
        (data) => {
          this.wallets = [...data.wallets];
          this.totalAmount = data.balance;
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
        },
        () => {
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      )
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((s) => {
      if (s) {
        s.unsubscribe();
      }
    });
    this.toastsService.clear();
  }
}
