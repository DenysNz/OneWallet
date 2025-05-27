import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificationEmailModalComponent } from './verification-email-modal.component';

describe('VerifyEmailModalComponent', () => {
  let component: VerificationEmailModalComponent;
  let fixture: ComponentFixture<VerificationEmailModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerificationEmailModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificationEmailModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
