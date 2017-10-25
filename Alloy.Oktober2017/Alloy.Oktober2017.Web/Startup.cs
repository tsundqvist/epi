using System;
using System.Web;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Alloy.Oktober2017.Web.Startup))]

namespace Alloy.Oktober2017.Web
{
	using System.ComponentModel.DataAnnotations.Schema;
	using EPiServer.Shell.Security;
	using Microsoft.AspNet.Identity.EntityFramework;

	public class Startup
    {

        public void Configuration(IAppBuilder app)
        {

            // Add CMS integration for ASP.NET Identity
			// todo Here:
            app.AddCmsAspNetIdentity<ApplicationUser>();

            // Remove to block registration of administrators
            app.UseAdministratorRegistrationPage(() => HttpContext.Current.Request.IsLocal);

            // Use cookie authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(Global.LoginPath),
                Provider = new CookieAuthenticationProvider
                {
					// If the "/util/login.aspx" has been used for login otherwise you don't need it you can remove OnApplyRedirect.
					// todo change here:
					OnApplyRedirect = cookieApplyRedirectContext =>
                    {
                        app.CmsOnCookieApplyRedirect(cookieApplyRedirectContext, 
							cookieApplyRedirectContext.OwinContext.Get<ApplicationSignInManager<ApplicationUser>>());
                    },

                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.
					
					// And then change here:
					// todo
                    OnValidateIdentity = 
					SecurityStampValidator
					.OnValidateIdentity<ApplicationUserManager<ApplicationUser>, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => manager.GenerateUserIdentityAsync(user))
                }
            });
        }
    }

	// This is exactly how u do extedneden normal microsoft idendentiyy
	// Way 1, Inherit from epis
	public class CustomUser : EPiServer.Cms.UI.AspNetIdentity.ApplicationUser
	{
		public string CustomProperty { get; set; }	
	}

	// Way 2, full custom, the IUIUser is epis itnerface so it works i guess
	public class CustomUser2 : IdentityUser, EPiServer.Shell.Security.IUIUser
	{
		public bool IsApproved { get; set; }
		public bool IsLockedOut { get; set; }
		public string PasswordQuestion { get; }
		public string Comment { get; set; }
		[Column(TypeName = "datetime2")]
		public DateTime CreationDate { get; }
		[Column(TypeName = "datetime2")]
		public DateTime? LastLoginDate { get; set; }
		[Column(TypeName = "datetime2")]
		public DateTime? LastLockoutDate { get; }

		public string ProviderName
		{
			get { return "MyProviderName"; }
		}

		[NotMapped]
		public string Username
		{
			get { return base.UserName; }
			set { base.UserName = value; }
		}

		// custom props:
		public string CustomProperty2 { get; set; }

	}
}
