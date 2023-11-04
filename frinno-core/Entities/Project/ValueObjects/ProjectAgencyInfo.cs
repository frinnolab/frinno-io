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
    public class ProjectAgencyInfo
    {
        [Column]
        public string CompanyAgencyName { get; set; } = string.Empty;
        [Column]
        public string CompanyAgencyPublicLink { get; set; } = string.Empty;

        [Column]
        public string CompanyAgencyMobile { get; set; }
        [Column]
        public string CompanyAgencyCity { get; set; }
        [Column]
        public string CompanyAgencyCountry { get; set; }
    }
}
