import { FormControl, FormGroup, Validators } from "@angular/forms";


export class LoginForm {
    static getFormGroup() {
        const formGroup = new FormGroup({
            userName: new FormControl('', Validators.required),
            password: new FormControl('', Validators.required)
        });

        return formGroup;
    }
}