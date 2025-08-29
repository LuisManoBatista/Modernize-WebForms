using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.UI;
using WebFormsApp.Identity.Managers;

namespace WebFormsApp.Account
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SignOut();
        }

        private void SignOut()
        {

            Context.GetAuthenticationManager().SignOut("Identity.Application");
            Response.Redirect("/");
        }
    }
}