using frinno_core.Entities.Profile.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frinno_core.Entities.Project.ValueObjects
{
    [Owned]
    public class ProjectClientInfo
    {
        [Column]
        public string ClientName { get; set; } = string.Empty;
        [Column]
        public string ClientPublicLink { get; set; }
        [Column]
        public string ClientMobile { get; set; }
        [Column]
        public string ClientCity { get; set; }
        [Column]
        public string ClientCountry { get; set; }
    }
}
