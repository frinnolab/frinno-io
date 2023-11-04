using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace frinno_core.Entities.Profile.ValueObjects
{
    [Owned]
    public class Address 
    {
        [Column]
        public string Mobile { get; set; }
        [Column]
        public string City { get; set; }
        [Column]
        public string Country { get; set; }
    }
}