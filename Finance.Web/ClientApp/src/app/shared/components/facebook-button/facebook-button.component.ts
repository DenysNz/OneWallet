import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { FacebookUserDetails } from 'src/app/core/models/facebook-user-details.model';
import { ApiService } from 'src/app/core/services/api.service';

@Component({
  selector: 'app-facebook-button',
  templateUrl: './facebook-button.component.html',
  styleUrls: ['./facebook-button.component.scss']
})
export class FacebookButtonComponent implements OnInit, OnDestroy {
  @Output() loginSucceded = new EventEmitter();
  @Output() loginFailed = new EventEmitter();
  subscriptions: Subscription [] = [];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
  }

  loginByFacebook() {
    (<any>window).FB.login((response: any) => {
      if (response.authResponse) {
        console.log("User login succeded");
        (<any>window).FB.api('/me', { fields: 'first_name,last_name,email,picture' }, (userInfo: any) => {
          this.subscriptions.push(this.apiService
            .authenticateViaSocialNetworks(new FacebookUserDetails(userInfo))
            .subscribe((res) => {
              this.loginSucceded.emit(res.token);
            }, (err) => {
              this.loginFailed.emit(err.error.errorMessage);
            }));
          (<any>window).FB.logout();
        });
      } else {
        console.log('User login failed', response);
      }
    }, { scope: 'public_profile, email' });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => {
      if(s) {
        s.unsubscribe();
      }
    })
  }
}
