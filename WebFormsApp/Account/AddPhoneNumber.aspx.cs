using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.UI;
using WebFormsApp.Identity.Managers;

namespace WebFormsApp.Account
{
    public partial class AddPhoneNumber : Page
    {
        protected void PhoneNumber_Click(object sender, EventArgs e)
        {
            var manager = Context.GetUserManager();
            var code = manager.GenerateChangePhoneNumberToken(User.Identity.GetUserId(), PhoneNumber.Text);
            if (manager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = PhoneNumber.Text,
                    Body = "Your security code is " + code
                };

                manager.SmsService.Send(message);
            }

            Response.Redirect("/Account/VerifyPhoneNumber?PhoneNumber=" + HttpUtility.UrlEncode(PhoneNumber.Text));
        }
    }
}