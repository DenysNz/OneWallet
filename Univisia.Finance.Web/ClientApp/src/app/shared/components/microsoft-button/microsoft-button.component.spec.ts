import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MicrosoftButtonComponent } from './microsoft-button.component';

describe('MicrosoftButtonComponent', () => {
  let component: MicrosoftButtonComponent;
  let fixture: ComponentFixture<MicrosoftButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MicrosoftButtonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MicrosoftButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
