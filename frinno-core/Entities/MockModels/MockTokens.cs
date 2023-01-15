using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities.MockModels
{
    public class MockTokens : MockBase
    {
        public int UserId { get; set; }
        public MockUser User { get; set; }

        public string Token { get; set; }

        public DateTime ValidUntil { get; set; }

        public bool IsValidToken { get; set; }
    }
}