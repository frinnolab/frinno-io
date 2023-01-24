using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using frinno_application.Projects;
using frinno_core.Entities.Projects;

namespace frinno_infrastructure.Repostories.ProjectsRepositories
{
    public class ProjectsRepository : IProjectsManager<Project>
    {
        public Project AddNew(Project newData)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project> GetAllBy(Expression<Func<Project, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Project GetSingle(Project data)
        {
            throw new NotImplementedException();
        }

        public Project GetSingleById(int dataId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int dataId)
        {
            throw new NotImplementedException();
        }

        public void SaveContextChanges()
        {
            throw new NotImplementedException();
        }

        public Project Update(Project updateData)
        {
            throw new NotImplementedException();
        }
    }
}