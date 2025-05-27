import { Component, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ApiService } from 'src/app/core/services/api.service';

@Component({
  selector: 'app-google-button',
  templateUrl: './google-button.component.html',
  styleUrls: ['./google-button.component.scss']
})
export class GoogleButtonComponent implements OnInit, OnDestroy {
  @ViewChild('google_button') googleButton!: ElementRef;
  @Output() loginSucceded = new EventEmitter();
  @Output() loginFailed = new EventEmitter();

  googleClientId: string = "";
  subscriptions: Subscription [] = [];

  constructor(private apiService: ApiService) { }


  ngOnInit(): void {
    this.getGoogleSettings();
  }

  getGoogleSettings() {
    this.subscriptions.push(this.apiService.getGoogleSettings()
    .subscribe((res) => {
      this.googleClientId = res.clientId;
      this.googleLogin();
    }));
  }

  googleLogin() {
    if((<any>window).google) {
      (<any>window).google.accounts.id.initialize({
        client_id: this.googleClientId,
        callback: this.handleGoogleLogin
      });
      (<any>window).google.accounts.id.prompt();
      (<any>window).google.accounts.id.renderButton(this.googleButton.nativeElement, {
        theme: 'outline',
        size: 'large',
        type: "standard",
        shape: "pill", 
        text: "signin_with",
        width: "219",
        logo_alignment: "left",
        locale: "en"
      });
    }
  }

  handleGoogleLogin = (response: any) => {
    this.subscriptions.push(this.apiService.authenticateViaGoogle(response.credential)
    .subscribe((res) => {
      this.loginSucceded.emit(res.token);
    }, 
    (err) => {
      this.loginFailed.emit(err.error.errorMessage);
    }));
  }


  ngOnDestroy(): void {
    this.subscriptions.forEach( s => {
      if (s) {
        s.unsubscribe();
      }
    });
  }
}
