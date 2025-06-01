export class UserLoginDetails {
    userName!: string;
    password!: string;
    
    constructor(model: any) {
        this.userName = model.userName;
        this.password = model.password;
    }
}