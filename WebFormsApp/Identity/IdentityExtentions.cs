using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Web;
using WebFormsApp.Identity.Managers;

namespace WebFormsApp
{
    public static class IdentityExtentions
    {
        // Used for XSRF when linking external logins
        public const string XsrfKey = "XsrfId";

        public const string ProviderNameKey = "providerName";
        public static string GetProviderNameFromRequest(this HttpRequest request)
        {
            return request.QueryString[ProviderNameKey];
        }

        public const string CodeKey = "code";
        public static string GetCodeFromRequest(HttpRequest request)
        {
            return request.QueryString[CodeKey];
        }

        public const string UserIdKey = "userId";
        public static string GetUserIdFromRequest(HttpRequest request)
        {
            return HttpUtility.UrlDecode(request.QueryString[UserIdKey]);
        }

        public static string GetResetPasswordRedirectUrl(string code, HttpRequest request)
        {
            var absoluteUri = "/Account/ResetPassword?" + CodeKey + "=" + HttpUtility.UrlEncode(code);
            return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
        }

        public static string GetUserConfirmationRedirectUrl(string code, string userId, HttpRequest request)
        {
            var absoluteUri = "/Account/Confirm?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
            return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
        }

        private static bool IsLocalUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }

        public static void RedirectToReturnUrl(string returnUrl, HttpResponse response)
        {
            if (!String.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
            {
                response.Redirect(returnUrl);
            }
            else
            {
                response.Redirect("~/");
            }
        }

        public static ApplicationUserManager GetUserManager(this HttpContext httpContext)
        {
            return httpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public static IAuthenticationManager GetAuthenticationManager(this HttpContext httpContext)
        {
            return httpContext.GetOwinContext().Authentication;
        }

        public static ApplicationSignInManager GetSignInManager(this HttpContext httpContext)
        {
            return httpContext.GetOwinContext().Get<ApplicationSignInManager>();
        }

        public static ExternalLoginInfo GetExternalLoginInfo(this HttpContext httpContext)
        {
            return httpContext.GetOwinContext().Authentication.GetExternalLoginInfo();
        }

        public static ExternalLoginInfo GetExternalLoginInfo(this HttpContext httpContext, string xsrfKey, string expectedValue)
        {
            return httpContext.GetOwinContext().Authentication.GetExternalLoginInfo(xsrfKey, expectedValue);
        }
    }
}

