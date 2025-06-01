import { FormControl, FormGroup, Validators } from "@angular/forms";


export class RegisterFrom {
    static passwordPattern: string | RegExp = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&^\(\)\-_+=,.\/])[A-Za-z\d$@$!%*#?&^\(\)\-_+=,.\/]{6,}$/;

    static getFormGroup() {
        const formGroup = new FormGroup({
            firstName: new FormControl('', Validators.required),
            lastName: new FormControl('', Validators.required),
            email: new FormControl('', [Validators.required, Validators.email]),
            password: new FormControl('', [Validators.required, Validators.pattern(this.passwordPattern)]),
            confirm: new FormControl('', Validators.required)
        });

        return formGroup;
    }
}