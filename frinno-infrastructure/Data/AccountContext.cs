using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.DTOs;
using frinno_core.Entities.AccountUser;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Data
{
    public class AccountContext : IdentityDbContext<AccountUser>
    {

        public AccountContext(DbContextOptions<IdentityDbContext> options):base(options)
        {
            
        }

        public DbSet<LoginResponse> Logins { get; set; }
    }
}