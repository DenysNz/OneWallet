export class FacebookUserDetails {
    firstName!: string;
    lastName!: string;
    userName!: string;
    email!: string;
    avatarUrl?: string;

    constructor(model: any) {
        this.firstName = model.first_name;
        this.lastName = model.last_name;
        this.email = model.email;
        this.userName = this.email.substring(0,this.email.indexOf("@")).replace(/[^\w\s]/gi,'') + 'Facebook';
        this.avatarUrl = model.picture.data.url; 
    }
}