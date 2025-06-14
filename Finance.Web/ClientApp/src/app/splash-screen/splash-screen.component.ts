import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-splash-screen',
  templateUrl: './splash-screen.component.html',
  styleUrls: ['./splash-screen.component.scss'],
})
export class SplashScreenComponent implements OnInit {

  windowWidth: string = "0";
  showSplash: boolean = true;

  constructor() { }

  ngOnInit(): void {
    setTimeout(() => {
      this.windowWidth = `-${window.innerWidth}px`;
      setTimeout(() => {
        this.showSplash = !this.showSplash;
      }, 1500)
    }, 3000);
  }

}
