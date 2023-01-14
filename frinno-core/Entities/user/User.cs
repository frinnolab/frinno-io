using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace frinno_core.Entities.user
{
    [Owned]
    public class User
    {
        [Column]
        public string Email { get; set; }
        [Column]
        public string Password { get; set; }
        [NotMapped]
        public string coPassword { get; set; }
    }
}