using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Okta.Sdk;

namespace Okta_Test.Account
{
    public partial class DeleteUser : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
       
            var client = new OktaClient(
             new Okta.Sdk.Configuration.OktaClientConfiguration
             {
                 DisableHttpsCheck = true,
                 OktaDomain = Properties.Settings.Default.OktaUrl,
                 Token = Properties.Settings.Default.OktaAPIToken,
             });
            var retrievedById = await client.Users.GetUserAsync(Request.Params["user_id"]);
            await retrievedById.DeactivateOrDeleteAsync();
            Response.Redirect("/Account/ManageUsers");
        }
    }
}