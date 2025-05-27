// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  clientId: "13f9bf14-4dac-4760-8a0e-47d76e21116b",
  recaptcha: {
    siteKey: '6LcLDTYkAAAAAIo3o6WWbtzhzS77dcAY4oppDj-2',
  },
  msalConfig: {
    auth: {
      clientId: "141106c7-77b9-4d21-8e5f-92c97d32e777",
      authority: "https://login.microsoftonline.com/common",
      redirectUri: "https://1wallet.pro/accounts"
    }
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
