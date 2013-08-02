using System.Web;
using System.Web.Security;

namespace CMZero.Web.Services.Login
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string organisationId, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(organisationId, true);
        }

        public string GetLoggedInOrganisationId()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie == null) return string.Empty;

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            if (ticket == null) return string.Empty;

            return ticket.Name;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}