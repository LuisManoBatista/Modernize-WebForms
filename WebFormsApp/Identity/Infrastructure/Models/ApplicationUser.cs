using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using WebFormsApp.Identity.Managers;

namespace WebFormsApp.Identity.Infrastructure.Models
{
    // You can add User data for the user by adding more properties to your User class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ClaimsIdentity GenerateUserIdentity(ApplicationUserManager manager)
        {
            var userIdentity = manager.CreateIdentity(this, "Identity.Application");
            // Add custom user claims here
            return userIdentity;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return Task.FromResult(GenerateUserIdentity(manager));
        }
    }
}
