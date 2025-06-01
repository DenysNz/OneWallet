import { FormControl, FormGroup, Validators } from "@angular/forms";

export class ResendEmailForm {
    static GetFormGroup() {
        const formGroup = new FormGroup({
            email: new FormControl('', [Validators.required, Validators.email])
        });

        return formGroup;
    }
}