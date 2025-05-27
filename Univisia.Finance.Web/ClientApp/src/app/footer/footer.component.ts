import {
  Component,
  ComponentFactoryResolver,
  EventEmitter,
  OnInit,
  Output,
  ViewContainerRef,
} from '@angular/core';
import {
  Router,
  NavigationStart,
  Event as NavigationEvent,
} from '@angular/router';
import { NgEventBus } from 'ng-event-bus';
import { AppSettings } from 'src/app-settings';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
})
export class FooterComponent implements OnInit {
  subscriptions: any[] = [];
  isVisible: boolean = false;

  constructor(
    private eventBus: NgEventBus,
    private router: Router,
    public viewContainerRef: ViewContainerRef
  ) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_SHOW_FOOTER_PLUS).subscribe(() => {
        this.isVisible = true;
      })
    );
    this.subscriptions.push(
      this.eventBus.on(AppSettings.EVENT_HIDE_FOOTER_PLUS).subscribe(() => {
        this.isVisible = false;
      })
    );
  }

  onOpenModal() {
    this.eventBus.cast(AppSettings.EVENT_ADD_BUTTON_CLICKED);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((subscription) => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }
}
