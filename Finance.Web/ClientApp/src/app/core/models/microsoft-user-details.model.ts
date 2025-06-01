export class MicrosoftUserDetails {
    firstName!: string;
    lastName!: string;
    userName!: string;
    email!: string;
    
    constructor(model: any) {
        this.firstName = model.name.substring(0, model.name.indexOf(' '));
        this.lastName = model.name.substring(model.name.indexOf(' ') + 1, model.name.length);
        this.userName = this.firstName + this.lastName + 'Microsoft';
        this.email = model.username;
    }
}