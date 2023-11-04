using frinno_core.Entities.Profiles;

namespace frinno_application.Authentication
{
    public interface IAuthService : ILoginService, IRegisterService
    {
        
        string GetAuthToken(Profile profile);
    }

}