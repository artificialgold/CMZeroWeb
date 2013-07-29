using CMZero.Web.Models;

namespace CMZero.Web.Services.Login
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, string organisationId, string roles, bool createPersistentCookie);

        void SignOut();

        bool IsAuthenticated();

        AuthenticatedUserDetails GetAuthenticatedUserData();
    }
}