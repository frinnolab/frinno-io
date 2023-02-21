using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Generics;

namespace frinno_application.Projects
{
    public interface IProjectsManager <Project> : IMasterService<frinno_core.Entities.Projects.Project>
    {
        List<Project> FetchAllByProfileId(string profileId);

        bool Exists(int projectId);
    }
}