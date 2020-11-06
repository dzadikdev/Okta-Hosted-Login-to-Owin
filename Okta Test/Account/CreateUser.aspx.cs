using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Okta.Sdk;

namespace Okta_Test.Account
{
    public partial class CreateUser : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/Sorry", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
          
            string userid = Request.Params["user_id"];
            if (userid == "" || userid == null)
            {
                Label1.Text = "Create User";
                UpdateButton.Visible = false;
                CheckBoxList1.Visible = false;

            }
            else
            {
                Label1.Text = "Update User";
                CreateButton.Visible = false;
                PasswordText.Visible = false;
                ConfirmText.Visible = false;
                Validate1.Enabled = false;
                Validate2.Enabled = false;
                var client = new OktaClient(
                new Okta.Sdk.Configuration.OktaClientConfiguration
                {
                    DisableHttpsCheck = true,
                    OktaDomain = Properties.Settings.Default.OktaUrl,
                    Token = Properties.Settings.Default.OktaAPIToken,
                });
                if (!IsPostBack)
                {
                    var UserInfo = await client.Users.GetUserAsync(Request.Params["user_id"]);
                    firstname.Text = UserInfo.Profile.FirstName;
                    lastname.Text = UserInfo.Profile.LastName;
                    Email.Text = UserInfo.Profile.Email;
                    Password.Visible = false;
                    ConfirmPassword.Visible = false;
                    Session.Add("user_id", Request.Params["user_id"]);
                    var groupList = await client.Groups.ListGroups().ToArray();

                    foreach (var group in groupList)
                    {
                        ListItem nli = new ListItem();
                        nli.Text = group.Profile.Name;
                        nli.Value = group.Id;
                        var groups = await UserInfo.Groups.ToList();
                        if (group.Profile.Name == "Everyone")
                            {
                            nli.Enabled = false;
                        }

                         foreach (IGroup mygroup in groups)
                        {
                            if (mygroup.Id == group.Id)
                            {
                                nli.Selected = true;
                                break;
                            }
                        }
                        CheckBoxList1.Items.Add(nli);
                    }
                }
            }
           
        }

        protected async void CreateUser_Click(object sender, EventArgs e)
        {

          var client = new OktaClient(
              new Okta.Sdk.Configuration.OktaClientConfiguration
              {
                  DisableHttpsCheck = true,
                  OktaDomain = Properties.Settings.Default.OktaUrl,
                  Token = Properties.Settings.Default.OktaAPIToken,
              });


            // Create a user
            var createdUser = await client.Users.CreateUserAsync(new CreateUserWithPasswordOptions
            {
                Profile = new UserProfile
                {
                    FirstName = firstname.Text,
                    LastName = lastname.Text,
                    Email = Email.Text,
                    Login = Email.Text,
                },
                Password = Password.Text,
                Activate = false,
            });

            try
            {
                // Activate the user
                await createdUser.ActivateAsync(sendEmail: true);
                Response.Redirect("/Account/ManageUsers", false);
            }
            catch (OktaApiException apiException)
            {
                Console.WriteLine(apiException.Message);

            }



        }
        protected async void UpdateUser_Click(object sender, EventArgs e)
        {

            var client = new OktaClient(
            new Okta.Sdk.Configuration.OktaClientConfiguration
            {
                DisableHttpsCheck = true,
                OktaDomain = Properties.Settings.Default.OktaUrl,
                Token = Properties.Settings.Default.OktaAPIToken,
            });

            var UserInfo = await client.Users.GetUserAsync(Session["user_id"].ToString());
            UserInfo.Profile.FirstName = firstname.Text;
            UserInfo.Profile.LastName = lastname.Text;
            UserInfo.Profile.Email = Email.Text;

            var updatedUser = await client.Users.UpdateUserAsync(UserInfo, UserInfo.Id, false);

            for (int i = 0; i <= (CheckBoxList1.Items.Count - 1); i++)
            {
                if (CheckBoxList1.Items[i].Selected)
                {
                    Boolean test = false;
                    var groups = await UserInfo.Groups.ToList();
                    foreach (IGroup mygroup in groups)
                    {
                        if (mygroup.Id == CheckBoxList1.Items[i].Value)
                        {
                            test = true;
                            break;
                        }
                    }
                    if (test == false)
                    {
                        var user = await client.Users.FirstOrDefault(x => x.Profile.Email == UserInfo.Profile.Email);

                        // find the desired group
                        var group = await client.Groups.FirstOrDefault(x => x.Profile.Name == CheckBoxList1.Items[i].Text);

                        // add the user to the group by using their id's
                        if (group != null && user != null)
                        {
                            await client.Groups.AddUserToGroupAsync(group.Id, user.Id);
                        }
                    }
                }
                else
                {
                    var groups = await UserInfo.Groups.ToList();
                    foreach (IGroup mygroup in groups)
                    {
                        if (mygroup.Id == CheckBoxList1.Items[i].Value)
                        {
                            var user = await client.Users.FirstOrDefault(x => x.Profile.Email == UserInfo.Profile.Email);

                            // find the desired group
                            var group = await client.Groups.FirstOrDefault(x => x.Profile.Name == CheckBoxList1.Items[i].Text);

                            // add the user to the group by using their id's
                            if (group != null && user != null)
                            {
                                await client.Groups.RemoveGroupUserAsync(group.Id, user.Id);
                            }
                            break;
                        }
                    }
                    
                }
            }
            Response.Redirect("/Account/ManageUsers",false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}