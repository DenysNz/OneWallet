import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RejectLoanDialodComponent } from './reject-loan-dialod.component';

describe('RejectLoanDialodComponent', () => {
  let component: RejectLoanDialodComponent;
  let fixture: ComponentFixture<RejectLoanDialodComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RejectLoanDialodComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RejectLoanDialodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
