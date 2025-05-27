import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResendVerificationEmailModalComponent } from './resend-verification-email-modal.component';

describe('ResendVerifyEmailModalComponent', () => {
  let component: ResendVerificationEmailModalComponent;
  let fixture: ComponentFixture<ResendVerificationEmailModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ResendVerificationEmailModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ResendVerificationEmailModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
