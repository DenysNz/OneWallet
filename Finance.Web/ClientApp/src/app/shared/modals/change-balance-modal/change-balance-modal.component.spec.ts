import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeBalanceModalComponent } from './change-balance-modal.component';

describe('ChangeBalanceModalComponent', () => {
  let component: ChangeBalanceModalComponent;
  let fixture: ComponentFixture<ChangeBalanceModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeBalanceModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeBalanceModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
