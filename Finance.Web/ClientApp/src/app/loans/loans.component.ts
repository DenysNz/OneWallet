import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { LoanModel } from 'src/app/core/models/loan.model';
import { LoansService } from 'src/app/core/services/loans.service';
import { LoanStatus } from '../core/enums/loan-status-id.enum';
import { ToastService } from '../core/services/toasts.service';
import { AddLoanTransactionComponent } from '../shared/modals/add-loan-transaction/add-loan-transaction.component';
import { DeleteDialogComponent } from '../shared/modals/delete-dialog/delete-dialog.component';
import { LoanDialogComponent } from '../shared/modals/loan-dialog/loan-dialog.component';
import { RejectLoanDialodComponent } from '../shared/modals/reject-loan-dialod/reject-loan-dialod.component';

@Component({
  selector: 'app-loans',
  templateUrl: './loans.component.html',
  styleUrls: ['./loans.component.scss'],
})
export class LoansComponent implements OnInit, OnDestroy {
  loans: LoanModel[] = [];
  subscriptions: any[] = [];
  serverErrorMessage: string = '';
  loanDeletedMessage: string = '';
  loanDeletedErrorMessage: string = '';
  noLoansMessage: string = '';
  loanUpdatedMessage: string = '';
  loanUpdatedErrorMessage: string = '';
  loanrejectedMessage: string = '';
  public loanStatus = LoanStatus;

  constructor(
    private loansService: LoansService,
    private eventBus: NgEventBus,
    private modalService: NgbModal,
    private toastsService: ToastService,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.translate.stream('TOAST').subscribe((value) => {
        this.serverErrorMessage = value.ERROR;
        this.loanDeletedMessage = value.LOANDELETED;
        this.loanDeletedErrorMessage = value.LOANDELETEDERROR;
        this.noLoansMessage = value.NOLOANS;
        this.loanUpdatedMessage = value.LOANUPDATED;
        this.loanUpdatedErrorMessage = value.LOANUPDATEDERROR;
        this.loanrejectedMessage = value.LOANREJECTED;
      })
    );
    this.loadLoans();
    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_ADD_BUTTON_CLICKED).subscribe(() => {
        this.loans.length
          ? this.modalService.open(AddLoanTransactionComponent, {
              centered: true,
            })
          : this.toastsService.showDanger(this.noLoansMessage, 5000);
      })
    );
    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_SAVE_BUTTON_CLICKED).subscribe(() => {
        this.loadLoans();
      })
    );
  }

  onChangeStatus(loan: any) {
    this.loansService.approveLoanRequest(loan.loanId).subscribe(
      () => {
        this.loadLoans();
        this.eventBus.cast(AppSettings.EVENT_COUNT_RESTART);
        this.toastsService.showSuccess(this.loanUpdatedMessage, 5000);
      },
      () => {
        this.toastsService.showDanger(this.serverErrorMessage, 5000);
      }
    );
  }

  onRejectLoanDialod(loan: any) {
    const modalRef = this.modalService.open(RejectLoanDialodComponent, {
      centered: true,
    });
    modalRef.componentInstance.object = loan;
    modalRef.result.then((result) => {
      this.loansService.rejectLoanRequest(result).subscribe(
        () => {
          this.loadLoans();
          this.eventBus.cast(AppSettings.EVENT_COUNT_RESTART);
          this.toastsService.showSuccess(this.loanrejectedMessage, 5000);
        },
        () => {
          this.toastsService.showDanger(this.serverErrorMessage, 5000);
        }
      );
    });
  }

  loadLoans() {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.subscriptions.push(
      this.loansService.getLoans().subscribe(
        (data) => {
          this.loans = [...data];
          if (data.length) {
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

  onAddLoan() {
    const modalRef = this.modalService.open(LoanDialogComponent, {
      centered: true,
    });
  }

  onUpdateLoan(loan: LoanModel) {
    const modalRef = this.modalService.open(LoanDialogComponent, {
      centered: true,
    });
    modalRef.componentInstance.loan = loan;
  }

  onDeleteLoan(loan: LoanModel) {
    const modalRef = this.modalService.open(DeleteDialogComponent, {
      centered: true,
    });
    modalRef.componentInstance.object = loan;
    modalRef.result.then((result) => {
      this.subscriptions.push(
        this.loansService.deleteLoan(result).subscribe(
          () => {
            this.loadLoans();
            this.toastsService.showSuccess(this.loanDeletedMessage, 5000);
          },
          () => {
            this.toastsService.showDanger(this.loanDeletedErrorMessage, 5000);
          }
        )
      );
    });
  }

  amountSymbolCheck(amount: number) {
    return Math.sign(amount) === 1;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((subscription) => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
    this.toastsService.clear();
    this.eventBus.cast(AppSettings.EVENT_HIDE_FOOTER_PLUS);
  }
}
