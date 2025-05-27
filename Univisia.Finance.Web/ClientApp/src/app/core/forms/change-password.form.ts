import { FormControl, FormGroup, Validators } from "@angular/forms";

export class ChangePasswordForm {
    static passwordPattern: string | RegExp = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$/;

    static GetFormGroup() {
        const formGroup = new FormGroup({
            oldPassword: new FormControl('', Validators.required),
            newPassword: new FormControl('', [Validators.required, Validators.pattern(this.passwordPattern)]),
            confirm: new FormControl(''),
        });

        return formGroup;
    }
}