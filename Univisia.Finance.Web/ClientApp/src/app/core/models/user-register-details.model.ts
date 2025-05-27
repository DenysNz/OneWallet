export class UserRegisterDetails {
    firstName!: string;
    lastName!: string;
    userName!: string;
    email!: string;
    password!: string;
    confirmPassword!: string

    constructor(model: any)
    {
        this.firstName = model.firstName;
        this.lastName = model.lastName;
        this.userName = model.email;
        this.email = model.email;
        this.password = model.password;
        this.confirmPassword = model.confirm;
    }
}