using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.UI;
using WebFormsApp.Identity.Managers;

namespace WebFormsApp.Account
{
    public partial class Confirm : Page
    {
        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = IdentityExtentions.GetCodeFromRequest(Request);
            string userId = IdentityExtentions.GetUserIdFromRequest(Request);
            if (code != null && userId != null)
            {
                var manager = Context.GetUserManager();
                var result = manager.ConfirmEmail(userId, code);
                if (result.Succeeded)
                {
                    successPanel.Visible = true;
                    return;
                }
            }
            successPanel.Visible = false;
            errorPanel.Visible = true;
        }
    }
}