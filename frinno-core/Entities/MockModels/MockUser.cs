using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities.MockModels
{
    public class MockUser : MockBase
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] confirmPassword { get; set; }

        public MockRoles Role { get; set; }
    }
}