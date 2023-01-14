using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities
{
    public class BaseEntity
    {
        [Key]
        public int ID { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}