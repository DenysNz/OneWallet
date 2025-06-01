export class UserDetails {
    firstName!: string;
    lastName!: string;
    userName!: string;
    email!: string;
    description!: string;
    isLoggedBySocialNetwork!: boolean

    constructor(model: any) {
        this.firstName = model.firstName;
        this.lastName = model.lastName;
        this.userName = model.userName;
        this.email = model.email;
        this.description = model.description;
        this.isLoggedBySocialNetwork = model.isLoggedBySocialNetwork;
    }
}