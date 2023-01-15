using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.MockModels;
using frinno_core.Entities.MockModels.MockDTOs;

namespace frinno_application.Generics
{
    public interface IMockAuthService
    {
        MockRegisterResponse RegisterUser(MockRegisterRequest request, MockRoles role);
        MockLoginResponse AuthenticateUser(MockUser user);
        bool ValidateUserPassword(MockLoginRequest loginRequest,MockUser user);
        PasswordHasModel HashedPassword(MockLoginRequest request);
        MockTokenResponse CreateToken(MockUser user);
        void RevokeToken(string apiToken);

        List<MockUser> GetMockUsers();
        MockUser GetMockUserByEmail(string email);
    }
}