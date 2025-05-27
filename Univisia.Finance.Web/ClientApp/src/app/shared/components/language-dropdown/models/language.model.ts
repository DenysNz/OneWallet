export class LanguageModel {
    translateName: string;
    id: number;
    fullName: string;
    imgName: string;

    constructor(translateName: string, id: number, fullName: string, imgName: string) {
        this.translateName = translateName
        this.id = id;
        this.fullName = fullName;
        this.imgName = imgName;
    }
}