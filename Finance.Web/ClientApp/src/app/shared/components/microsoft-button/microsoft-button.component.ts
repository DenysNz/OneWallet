import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { Subscription } from 'rxjs';
import { MicrosoftUserDetails } from 'src/app/core/models/microsoft-user-details.model';
import { ApiService } from 'src/app/core/services/api.service';

@Component({
  selector: 'app-microsoft-button',
  templateUrl: './microsoft-button.component.html',
  styleUrls: ['./microsoft-button.component.scss']
})
export class MicrosoftButtonComponent implements OnInit, OnDestroy {
  @Output() loginSucceded = new EventEmitter();
  @Output() loginFailed = new EventEmitter();
  subscription: Subscription [] = [];

  constructor(private msalService: MsalService,
    private apiService: ApiService) { }

  ngOnInit(): void { }

  loginByMicrosoft() {
    this.subscription.push(this.msalService.loginPopup()
    .subscribe((res) => {
      this.subscription.push(this.apiService
        .authenticateViaSocialNetworks(new MicrosoftUserDetails(res.account))
        .subscribe((res)=> {
          this.loginSucceded.emit(res.token);
        }, (err) => {
          this.loginFailed.emit(err.error.errorMessage);
        }));
    }, (err) => {
      console.log(err);
    }));
  }

  ngOnDestroy(): void {
    this.subscription.forEach(s => {
      if (s) {
        s.unsubscribe();
      }
    });
  }
}
