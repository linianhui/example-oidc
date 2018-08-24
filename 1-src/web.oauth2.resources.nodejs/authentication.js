const jwt = require('express-jwt');
const jwks = require('jwks-rsa');

module.exports = jwt({
  secret: jwks.expressJwtSecret({
    cache: true,
    rateLimit: true,
    jwksRequestsPerMinute: 2,
    jwksUri: "http://oidc-server.test/.well-known/openid-configuration/jwks"
  }),
  audience: "api-2",
  issuer: "http://oidc-server.test"
});