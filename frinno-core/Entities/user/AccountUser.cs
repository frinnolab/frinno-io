using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace frinno_core.Entities.AccountUser
{
    public class AccountUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}