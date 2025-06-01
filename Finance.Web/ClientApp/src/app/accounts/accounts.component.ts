import {
  Component,
  OnDestroy,
  OnInit,
  ChangeDetectorRef,
  AfterContentChecked,
  ViewContainerRef,
} from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgEventBus } from 'ng-event-bus';
import { Subscription } from 'rxjs';
import { AppSettings } from 'src/app-settings';
import { AccountModel } from 'src/app/core/models/account.model';
import { AccountService } from 'src/app/core/services/account.service';
import { ToastService } from '../core/services/toasts.service';
import { AccountDialogComponent } from '../shared/modals/account-dialog/account-dialog.component';
import { ChangeBalanceModalComponent } from '../shared/modals/change-balance-modal/change-balance-modal.component';
import { DeleteDialogComponent } from '../shared/modals/delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss'],
})
export class AccountsComponent
  implements OnInit, OnDestroy, AfterContentChecked
{
  accounts: AccountModel[] = [];
  subscriptions: any[] = [];
  showDeleted: boolean = false;
  noAccountsMessage: string = '';
  serverErrorMessage: string = '';
  accountDeletedMessage: string = '';
  accountDeletedErrorMessage: string = '';

  constructor(
    private accountService: AccountService,
    private eventBus: NgEventBus,
    private cdRef: ChangeDetectorRef,
    public viewContainerRef: ViewContainerRef,
    private modalService: NgbModal,
    private toastsService: ToastService,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.translate.stream('TOAST').subscribe((value) => {
        this.noAccountsMessage = value.NOACCOUNTS;
        this.serverErrorMessage = value.ERROR;
        this.accountDeletedMessage = value.ACCDELETED;
        this.accountDeletedErrorMessage = value.ACCDELETEDERROR;
      })
    );
    this.loadAccounts();
    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_ADD_BUTTON_CLICKED).subscribe(() => {
        this.accounts.length
          ? this.modalService.open(ChangeBalanceModalComponent, {
              centered: true,
            })
          : this.toastsService.showDanger(this.noAccountsMessage, 5000);
      })
    );
    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_SAVE_BUTTON_CLICKED).subscribe(() => {
        this.loadAccounts();
      })
    );
  }

  onUpdateAccount(account: AccountModel) {
    const modalRef = this.modalService.open(AccountDialogComponent, {
      centered: true,
    });
    modalRef.componentInstance.object = account;
  }

  loadAccounts() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.subscriptions.push(
      this.accountService.getAccounts(this.showDeleted).subscribe(
        (data) => {
          this.accounts = [...data];
          if (data.length && !this.showDeleted) {
            this.eventBus.cast(AppSettings.EVENT_SHOW_FOOTER_PLUS);
          } else {
            this.eventBus.cast(AppSettings.EVENT_HIDE_FOOTER_PLUS);
          }
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
        },
        () => {
          this.eventBus.cast(AppSettings.EVENT_SPINNER_HIDE);
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      )
    );
  }

  onShowDeletedAccounts(): void {
    this.loadAccounts();
  }

  onOpenDeleteModal(acc: AccountModel): void {
    const modalRef = this.modalService.open(DeleteDialogComponent, {
      centered: true,
    });
    modalRef.componentInstance.object = acc;
    modalRef.result.then(
      (result) => {
        this.subscriptions.push(
          this.accountService.deleteAccount(result).subscribe(
            () => {
              this.loadAccounts();
              this.toastsService.showSuccess(this.accountDeletedMessage, 5000);
            },
            () => {
              this.toastsService.showDanger(
                this.accountDeletedErrorMessage,
                5000
              );
            }
          )
        );
      },
      (reason) => {}
    );
  }

  onAddAccount() {
    this.modalService.open(AccountDialogComponent, {
      centered: true,
    });
  }

  ngAfterContentChecked(): void {
    this.cdRef.detectChanges();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((s) => {
      if (s) {
        s.unsubscribe();
      }
    });
    this.toastsService.clear();
    this.eventBus.cast(AppSettings.EVENT_HIDE_FOOTER_PLUS);
  }
}
