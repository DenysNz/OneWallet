const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:11449';

const PROXY_CONFIG = [
  {
    context: [
      "/authentication/init",
      "/authentication/login",
      "/authentication/register",
      "/authentication/changepassword",
      "/authentication/sendsecuritycode",
      "/authentication/verifysecuritycode",
      "/authentication/resetpassword",
      "/social/googlesettings",
      "/social/googlelogin",
      "/social/socialnetworklogin",
      "/userdetails/get",
      "/userdetails/update",
      "/userdetails/statuswelcome",
      "/userdetails/updatestatuswelcome",
      "/loan/loans",
      "/loan/update",
      "/loan/delete",
      "/loan/add",
      "/loan/operation",
      "/loan/approveloan",
      "/loan/rejectloan",
      "/loan/getrequestcount",
      "/account/accounts",
      "/account/add",
      "/account/delete",
      "/account/update",
      "/account/changebalance",
      "/accounts/balance",
      "/account/lookup",
      "/account/wallet",
      "/currency/lookup",
      "/currency/usercurrencies",
      "/currency/saveusercurrencies",
      "/transaction/transactions",
      "/transaction/transactionscount",
      "/support/requestsupport",
      "/pushsubscription/pushsubscription"
,   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
