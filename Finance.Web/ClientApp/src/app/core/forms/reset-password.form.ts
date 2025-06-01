import { FormControl, FormGroup, Validators } from "@angular/forms";

export class ResetPasswordFrom {
    static passwordPattern: string | RegExp = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$/;

    static GetFormGroup() {
        const formGroup = new FormGroup({
            email: new FormControl('', [Validators.required, Validators.email]),
            password: new FormControl('',[Validators.required, Validators.pattern(this.passwordPattern)]),
            confirm: new FormControl('', [Validators.required])
        });

        return formGroup;
    }
}