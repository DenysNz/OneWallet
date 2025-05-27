import { FormControl, FormGroup, Validators } from "@angular/forms";

export class UserDetailsForm {
    static GetFormGroup() {
        const formGroup = new FormGroup({
            firstName: new FormControl('', Validators.required),
            lastName: new FormControl('', Validators.required),
            userName: new FormControl('', Validators.required),
            email: new FormControl('', [Validators.required, Validators.email]),
            description: new FormControl('')
        });

        return formGroup;
    }
}