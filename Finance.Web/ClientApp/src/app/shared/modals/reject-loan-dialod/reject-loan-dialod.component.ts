import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LoanModel } from 'src/app/core/models/loan.model';
import { ReasomObjectModel } from './models/reason-object.model';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-reject-loan-dialod',
  templateUrl: './reject-loan-dialod.component.html',
  styleUrls: ['./reject-loan-dialod.component.scss']
})
export class RejectLoanDialodComponent {
  @Input() object!: LoanModel;

  reasonForm: FormGroup;
  subscriptions: any[] = [];

  constructor(
      private activeModal: NgbActiveModal,
      public translate: TranslateService,
    ) {
      this.reasonForm = new FormGroup({
        text: new FormControl('', [Validators.required]),
      });
    }

  ngOnInit(): void {}

  onSave() {
    let value = this.reasonForm.value;
    let reasomObject = new ReasomObjectModel(
    this.object.loanId,
    value.text,
    );
    this.activeModal.close(reasomObject);
  }

  onDiscard() {
    this.activeModal.dismiss();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((subscription) => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }
}
