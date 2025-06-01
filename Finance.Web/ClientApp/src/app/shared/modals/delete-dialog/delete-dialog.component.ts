import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';
import { AccountModel } from 'src/app/core/models/account.model';

@Component({
  selector: 'app-delete-dialog',
  templateUrl: './delete-dialog.component.html',
  styleUrls: ['./delete-dialog.component.scss'],
})
export class DeleteDialogComponent implements OnInit {
  @Input() object: any;

  constructor(
      private activeModal: NgbActiveModal,
      private eventBus: NgEventBus
    ) {}

  ngOnInit(): void {}

  onSave(id: number) {
    this.eventBus.cast(AppSettings.EVENT_SPINNER_SHOW);
    this.activeModal.close(id);
  }

  onDiscard() {
    this.activeModal.dismiss();
  }
}
