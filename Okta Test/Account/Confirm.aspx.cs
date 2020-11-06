using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections;
using System.Web;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Owin.Security.Cookies;

namespace Okta_Test.Account
{
    public partial class Confirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
               HttpContext.Current.GetOwinContext().Authentication.Challenge(OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
            else
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                // Extract tokens
               
                if (!IsPostBack)
                {
                        string accessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
                        string idToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;
                        idtokendecoded.Text = "";
                        accesstokendecoded.Text = "";

                        var jwt = idToken;
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(jwt);

                        foreach (var claim in token.Claims)
                        {
                            idtokendecoded.Text = idtokendecoded.Text + claim.Type + ": " + claim.Value + "<br />";
                        }

                        var jwt2 = accessToken;
                        var handler2 = new JwtSecurityTokenHandler();
                        var token2 = handler2.ReadJwtToken(jwt2);

                        foreach (var claim in token2.Claims)
                        {
                            accesstokendecoded.Text = accesstokendecoded.Text + claim.Type + ": " + claim.Value + "<br />";
                        }
                }
            }
        }
        protected void logoout_Click(object sender, EventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            Response.Redirect(Properties.Settings.Default.OktaUrl + "/login/signout?fromURI=https://localhost:44319");
        }

    }
}