export class SupportModel {
  name: string;
  email: string;
  text: string;
  token: string | undefined;

  constructor(name: string, email: string, text: string, token: string | undefined) {
    this.token = token,
    this.name = name,
    this.email = email,
    this.text = text
  }
}
