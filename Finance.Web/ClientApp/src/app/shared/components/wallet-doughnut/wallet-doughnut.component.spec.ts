import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletDoughnutComponent } from './wallet-doughnut.component';

describe('WalletDoughnutComponent', () => {
  let component: WalletDoughnutComponent;
  let fixture: ComponentFixture<WalletDoughnutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletDoughnutComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletDoughnutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
