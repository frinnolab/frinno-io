using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities.Resumes
{
    public class Resume : BaseEntity
    {
        public string Title { get; set; }
        public Profiles.Profile Profile { get; set; }

        public FileAsset.FileAsset File { get; set; }

    }
}