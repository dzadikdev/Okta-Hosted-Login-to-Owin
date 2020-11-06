using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

using Owin;

[assembly: OwinStartup(typeof(Okta_Test.Startup1))]

namespace Okta_Test
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            ConfigureAuth(app);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
            });

            var clientId = Properties.Settings.Default.OktaAppId;
            var clientSecret = Properties.Settings.Default.OktaAppSecret;
            var issuer = Properties.Settings.Default.OktaUrl + "/oauth2/default";
            var redirectUri = "https://localhost:44319/Account/Confirm";

            _ = app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Authority = issuer,
                RedirectUri = redirectUri,
                ResponseType = "code id_token",
                UseTokenLifetime = false,
                Scope = "openid profile",
                PostLogoutRedirectUri = "",
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                },
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    RedirectToIdentityProvider = context =>
                    {
                        if (context.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                        {
                            var idToken = context.OwinContext.Authentication.User.Claims.FirstOrDefault(c => c.Type == "id_token")?.Value;
                            context.ProtocolMessage.IdTokenHint = idToken;
                        }

                        return Task.FromResult(true);
                    },
                    AuthorizationCodeReceived = async notification  =>
                    {
                        // Exchange code for access and ID tokens
                        var tokenClient = new TokenClient(
                            issuer + "/v1/token", clientId, clientSecret);
                        var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(notification.ProtocolMessage.Code, redirectUri);

                        if (tokenResponse.IsError)
                        {
                            throw new Exception(tokenResponse.Error);
                        }

                        var userInfoClient = new UserInfoClient(issuer + "/v1/userinfo");
                        var userInfoResponse = await userInfoClient.GetAsync(tokenResponse.AccessToken);

                        var id = new ClaimsIdentity(notification.AuthenticationTicket.Identity.AuthenticationType);
                        id.AddClaims(userInfoResponse.Claims);
                        id.AddClaim(new Claim("access_token", tokenResponse.AccessToken));
                        id.AddClaim(new Claim("id_token", tokenResponse.IdentityToken));

                        var nameClaim = new Claim(
                         ClaimTypes.Name,
                         userInfoResponse.Claims.FirstOrDefault(c => c.Type == "name")?.Value);
                        id.AddClaim(nameClaim);

                        System.Threading.Thread.CurrentPrincipal = new ClaimsPrincipal(id);

                        notification.AuthenticationTicket = new AuthenticationTicket(
                             new ClaimsIdentity(id.Claims, notification.AuthenticationTicket.Identity.AuthenticationType),
                             notification.AuthenticationTicket.Properties);
                        //notification.AuthenticationTicket.Properties.RedirectUri = redirectUri;
                    }
                }
            });
        }
    }
}
