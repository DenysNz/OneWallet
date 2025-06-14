import { Injectable } from '@angular/core';
import { AbstractControl, ValidatorFn } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class PasswordConfirmationValidator {

  constructor() { }

  public validateConfirmPassword = (passwordControl: AbstractControl): ValidatorFn => {
    return (confirmationControl: AbstractControl) : { [key: string]: boolean } | null => {

      const confirmValue = confirmationControl.value;
      const passwordValue = passwordControl.value;

      if (confirmValue !== passwordValue) {
        return  { mustMatch: true }
      } 

      return null;
    };
  }
}