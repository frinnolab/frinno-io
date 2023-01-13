using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Generics;

namespace frinno_application.Resumes
{
    public interface IResumesManager <Resume> : IMasterService<frinno_core.Entities.Resumes.Resume>
    {
        void DownloadResume(int resumeId);
    }
}