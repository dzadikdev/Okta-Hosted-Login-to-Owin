using Okta.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Okta_Test.Account
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/Sorry", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            var client = new OktaClient(
            new Okta.Sdk.Configuration.OktaClientConfiguration
            {
                 DisableHttpsCheck = true,
                 OktaDomain = Properties.Settings.Default.OktaUrl,
                 Token = Properties.Settings.Default.OktaAPIToken,
            });


            var foundUsers = await client.Users
                     .ListUsers()
                     .ToArray();

            TableRow myrow1 = new TableRow();
            TableCell mycell11 = new TableCell();
            TableCell mycell21 = new TableCell();
            TableCell mycell31 = new TableCell();
            TableCell mycell41 = new TableCell();
            TableCell mycell51 = new TableCell();
            mycell11.Text = "First Name";
            mycell21.Text = "Last Name";
            mycell31.Text = "Email";
            mycell41.Text = "";
            mycell51.Text = "";
            myrow1.Cells.Add(mycell11);
            myrow1.Cells.Add(mycell21);
            myrow1.Cells.Add(mycell31);
            myrow1.Cells.Add(mycell41);
            myrow1.Cells.Add(mycell51);
            Table1.Rows.Add(myrow1);

            foreach (var user in foundUsers)
            {
                
                TableRow myrow = new TableRow();
                TableCell mycell1 = new TableCell();
                TableCell mycell2 = new TableCell();
                TableCell mycell3 = new TableCell();
                TableCell mycell4 = new TableCell();
                TableCell mycell5 = new TableCell();
                mycell1.Text = user.Profile.FirstName.ToString();
                mycell2.Text = user.Profile.LastName.ToString();
                mycell3.Text = user.Profile.Email.ToString();
                if (user.Credentials.Provider.Name.ToString() == "OKTA")
                {
                    mycell4.Text = "<a href='/Account/CreateUser/?user_id=" + user.Id + "'>Update User</a>";
                    mycell5.Text = "<a href='/Account/DeleteUser/?user_id=" + user.Id + "'>Delete User</a>";

                }
                else
                {
                    mycell4.Text = "Update User";
                    mycell5.Text = "Delete User";
                }


                myrow.Cells.Add(mycell1);
                myrow.Cells.Add(mycell2);
                myrow.Cells.Add(mycell3);
                myrow.Cells.Add(mycell4);
                myrow.Cells.Add(mycell5);
                Table1.Rows.Add(myrow);
              }
        }
    }
}