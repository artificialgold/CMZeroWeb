using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

using CMZero.Web.Models;

namespace CMZero.Web.Services.Login
{
    //TODO: Assess whether this is really the best way to do this!
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, string organisationId, string roles, bool createPersistentCookie)
        {
            var ticket =
                new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(50), createPersistentCookie, string.Format("{0}[~]{1}", organisationId, roles), FormsAuthentication.FormsCookiePath);
            string hashCookies = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.User =
                new GenericPrincipal(
                    HttpContext.Current.User.Identity,
                    roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public bool IsAuthenticated()
        {
            return GetAuthenticatedUserData().IsAuthenticated;
        }

        public static AuthenticatedUserDetails GetAuthenticatedUserData()
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        var identity = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = identity.Ticket;

                        //TODO: Needs testing
                        string organisationId = ticket.UserData.Substring(0, ticket.UserData.IndexOf("[~]"));

                        string[] roles = ticket.UserData.Substring(ticket.UserData.IndexOf("[~]") + 3)
                            .Split(new[] { "[~]" }, StringSplitOptions.RemoveEmptyEntries);
                        HttpContext.Current.User = new GenericPrincipal(identity, roles);
                       return new AuthenticatedUserDetails{IsAuthenticated = true, OrganisationId = organisationId};
                    }
                }
            }

            return new AuthenticatedUserDetails{IsAuthenticated = false}; 
        }
    }
}