namespace CMZero.Web.Services.Login
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string organisationId, bool createPersistentCookie);
        string GetLoggedInOrganisationId();
        void SignOut();
    }
}